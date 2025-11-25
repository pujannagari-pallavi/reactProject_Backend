using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IEventService _eventService;
        private readonly IBookingService _bookingService;
        private readonly IVendorService _vendorService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public DashboardService(IEventService eventService, IBookingService bookingService, IVendorService vendorService, IUserService userService, INotificationService notificationService)
        {
            _eventService = eventService;
            _bookingService = bookingService;
            _vendorService = vendorService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<object> GetAdminDashboardAsync()
        {
            var events = await _eventService.GetAllAsync();
            var bookings = await _bookingService.GetAllAsync();
            var vendors = await _vendorService.GetAllAsync();
            var users = await _userService.GetAllAsync();

            var adminUsers = users.Where(u => u.Role == UserRole.Admin).ToList();
            var allNotifications = new List<Notification>();

            foreach (var admin in adminUsers)
            {
                var adminNotifications = await _notificationService.GetNotificationsByUserIdAsync(admin.Id);
                allNotifications.AddRange(adminNotifications);
            }

            // Return only summary data, not full entities to avoid large payload
            return new
            {
                TotalEvents = events?.Count() ?? 0,
                ActiveBookings = bookings?.Count(b => b.Status == BookingStatus.Confirmed) ?? 0,
                CompletedEvents = events?.Count(e => e.EventDate < DateTime.Now) ?? 0,
                TotalRevenue = bookings?.Sum(b => b.Amount) ?? 0,
                TotalVendors = vendors?.Count() ?? 0,
                VerifiedVendors = vendors?.Count(v => v.IsVerified) ?? 0,
                TotalUsers = users?.Count() ?? 0,
                AverageRating = vendors?.Where(v => v.Rating > 0).Average(v => v.Rating) ?? 0,
                Notifications = allNotifications.Take(10).Select(n => new { n.Id, n.Title, n.Message, n.CreatedAt, n.IsRead }),
                UnreadCount = allNotifications.Count(n => !n.IsRead)
            };
        }

        public async Task<ClientDashboardDto> GetClientDashboardAsync(int userId)
        {
            var events = await _eventService.GetEventsByUserIdAsync(userId) ?? new List<Event>();
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId) ?? new List<Booking>();
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId) ?? new List<Notification>();
            var vendors = await _vendorService.GetAllAsync() ?? new List<Vendor>();

            return new ClientDashboardDto
            {
                Events = events.ToList(),
                Bookings = bookings.ToList(),
                Notifications = notifications.Take(10).ToList(),
                UnreadCount = notifications.Count(n => !n.IsRead),
                Vendors = vendors.ToList()
            };
        }

        public async Task<object> GetVendorDashboardAsync(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId) ?? new List<Notification>();
            var allVendors = await _vendorService.GetAllAsync() ?? new List<Vendor>();
            var vendors = user != null ? allVendors.Where(v => v.Email == user.Email).ToList() : new List<Vendor>();

            return new
            {
                Notifications = notifications.Take(10),
                UnreadCount = notifications.Count(n => !n.IsRead),
                Vendors = vendors
            };
        }

        public async Task<object> GetStatsAsync()
        {
            var events = await _eventService.GetAllAsync();
            var bookings = await _bookingService.GetAllAsync();
            var vendors = await _vendorService.GetAllAsync();

            return new
            {
                TotalEvents = events?.Count() ?? 0,
                ActiveBookings = bookings?.Count(b => b.Status == BookingStatus.Confirmed) ?? 0,
                CompletedEvents = events?.Count(e => e.EventDate < DateTime.Now) ?? 0,
                TotalRevenue = bookings?.Sum(b => b.Amount) ?? 0,
                TotalVendors = vendors?.Count() ?? 0,
                VerifiedVendors = vendors?.Count(v => v.IsVerified) ?? 0
            };
        }

        public async Task<object> GetUserStatsAsync(int userId)
        {
            var userEvents = await _eventService.GetEventsByUserIdAsync(userId);
            var userBookings = userEvents?.SelectMany(e => e.Bookings ?? new List<Booking>())
                               ?? Enumerable.Empty<Booking>();

            return new
            {
                TotalEvents = userEvents?.Count() ?? 0,
                ActiveBookings = userBookings.Count(b => b.Status == BookingStatus.Confirmed),
                CompletedEvents = userEvents?.Count(e => e.EventDate < DateTime.Now) ?? 0,
                TotalRevenue = userBookings.Sum(b => b.Amount)
            };
        }

        public async Task<object> GetAnalyticsDataAsync()
        {
            var events = await _eventService.GetAllAsync();
            var bookings = await _bookingService.GetAllAsync();
            var vendors = await _vendorService.GetAllAsync();
            var users = await _userService.GetAllAsync();

            return new
            {
                TotalEvents = events?.Count() ?? 0,
                ActiveBookings = bookings?.Count(b => b.Status == BookingStatus.Confirmed) ?? 0,
                TotalRevenue = bookings?.Sum(b => b.Amount) ?? 0,
                TotalVendors = vendors?.Count() ?? 0,
                TotalUsers = users?.Count() ?? 0,
                AverageRating = vendors?.Where(v => v.Rating > 0).Average(v => v.Rating) ?? 0,
                Events = events?.Select(e => new { e.Id, e.EventType }).ToList(),
                Bookings = bookings?.Select(b => new { b.Id, b.Status }).ToList(),
                Vendors = vendors?.Select(v => new { v.Id, v.Category }).ToList()
            };
        }
    }
}
