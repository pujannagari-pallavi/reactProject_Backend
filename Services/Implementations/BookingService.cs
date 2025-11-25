using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class BookingService : GenericService<Booking>, IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventService _eventService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IEventService eventService, IEmailService emailService, IUserRepository userRepository, IVendorRepository vendorRepository, INotificationService notificationService, IMapper mapper) : base(bookingRepository)
        {
            _bookingRepository = bookingRepository;
            _eventService = eventService;
            _emailService = emailService;
            _userRepository = userRepository;
            _vendorRepository = vendorRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<Booking> CreateFromDtoAsync(BookingCreateDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);
            return await CreateAsync(booking);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId)
        {
            return await _bookingRepository.GetBookingsByEventIdAsync(eventId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByVendorIdAsync(int vendorId)
        {
            return await _bookingRepository.GetBookingsByVendorIdAsync(vendorId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepository.GetBookingsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
        {
            return await _bookingRepository.GetBookingsByStatusAsync(status);
        }

        public async Task<decimal> GetTotalBookingAmountByEventAsync(int eventId)
        {
            return await _bookingRepository.GetTotalBookingAmountByEventAsync(eventId);
        }

        public async Task<IEnumerable<Booking>> GetPendingPaymentsAsync()
        {
            return await _bookingRepository.GetPendingPaymentsAsync();
        }

        public override async Task<Booking> CreateAsync(Booking booking)
        {
            booking.CreatedAt = DateTime.UtcNow;
            booking.Status = BookingStatus.Pending;
            booking.PaymentStatus = PaymentStatus.Pending;
            var result = await base.CreateAsync(booking);
            await UpdateEventBudgetAsync(booking.EventId);
            
            // Send confirmation email
            try
            {
                var eventData = await _eventService.GetByIdAsync(booking.EventId);
                var user = await _userRepository.GetByIdAsync(eventData.UserId);
                var vendor = await _vendorRepository.GetByIdAsync(booking.VendorId);
                
                var emailBody = $@"
                    <h2>Booking Confirmation</h2>
                    <p>Dear {user.FirstName},</p>
                    <p>Your booking has been confirmed!</p>
                    <h3>Booking Details:</h3>
                    <ul>
                        <li><strong>Vendor:</strong> {vendor.BusinessName}</li>
                        <li><strong>Service:</strong> {booking.ServiceName}</li>
                        <li><strong>Event:</strong> {eventData.Title}</li>
                        <li><strong>Date:</strong> {booking.ServiceDate:dd MMM yyyy}</li>
                        <li><strong>Amount:</strong> ₹{booking.Amount:N0}</li>
                    </ul>
                    <p>Thank you for using Wedding Planner!</p>
                ";
                
                await _emailService.SendEmailAsync(user.Email, "Booking Confirmation - Wedding Planner", emailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send booking confirmation email: {ex.Message}");
            }
            
            return result;
        }

        public override async Task<Booking> UpdateAsync(Booking booking)
        {
            var existingBooking = await _bookingRepository.GetByIdAsync(booking.Id);
            var oldStatus = existingBooking?.Status;
            
            booking.UpdatedAt = DateTime.UtcNow;
            
            // Auto-update payment status based on paid amount
            if (booking.PaidAmount >= booking.Amount)
            {
                booking.PaymentStatus = PaymentStatus.Paid;
            }
            else if (booking.PaidAmount > 0)
            {
                booking.PaymentStatus = PaymentStatus.Partial;
            }
            else
            {
                booking.PaymentStatus = PaymentStatus.Pending;
            }
            
            var result = await base.UpdateAsync(booking);
            await UpdateEventBudgetAsync(booking.EventId);
            
            // Send notification if status changed
            if (oldStatus.HasValue && oldStatus.Value != booking.Status)
            {
                try
                {
                    var eventData = await _eventService.GetByIdAsync(booking.EventId);
                    if (eventData?.UserId != null)
                    {
                        var statusText = booking.Status switch
                        {
                            BookingStatus.Confirmed => "confirmed",
                            BookingStatus.Cancelled => "cancelled",
                            BookingStatus.Completed => "completed",
                            _ => "updated"
                        };
                        
                        await _notificationService.CreateAsync(new Notification
                        {
                            Title = $"Booking {statusText}",
                            Message = $"Your booking for {booking.ServiceName} has been {statusText}",
                            UserId = eventData.UserId,
                            Type = NotificationType.Booking,
                            Priority = NotificationPriority.Medium
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send notification: {ex.Message}");
                }
            }
            
            return result;
        }

        public async Task<Booking> PartialUpdateAsync(int id, BookingStatus? status, PaymentStatus? paymentStatus, decimal? paidAmount, string serviceName, string serviceDescription, decimal? amount, DateTime? serviceDate, string timeSlot, string paymentMethod, string notes)
        {
            var existingBooking = await _bookingRepository.GetByIdAsync(id);
            if (existingBooking == null) return null;

            if (status.HasValue) existingBooking.Status = status.Value;
            if (paymentStatus.HasValue) existingBooking.PaymentStatus = paymentStatus.Value;
            if (paidAmount.HasValue) existingBooking.PaidAmount = paidAmount.Value;
            if (!string.IsNullOrEmpty(serviceName)) existingBooking.ServiceName = serviceName;
            if (!string.IsNullOrEmpty(serviceDescription)) existingBooking.ServiceDescription = serviceDescription;
            if (amount.HasValue) existingBooking.Amount = amount.Value;
            if (serviceDate.HasValue) existingBooking.ServiceDate = serviceDate.Value;
            if (!string.IsNullOrEmpty(timeSlot)) existingBooking.TimeSlot = timeSlot;
            if (!string.IsNullOrEmpty(paymentMethod)) existingBooking.PaymentMethod = paymentMethod;
            if (!string.IsNullOrEmpty(notes)) existingBooking.Notes = notes;

            return await UpdateAsync(existingBooking);
        }

        private async Task UpdateEventBudgetAsync(int eventId)
        {
            var eventData = await _eventService.GetByIdAsync(eventId);
            if (eventData != null)
            {
                var totalSpent = await GetTotalBookingAmountByEventAsync(eventId);
                eventData.SpentAmount = totalSpent;
                await _eventService.UpdateAsync(eventData);
            }
        }
    }
}
