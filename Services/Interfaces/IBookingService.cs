using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IBookingService : IGenericService<Booking>
    {
        Task<Booking> CreateFromDtoAsync(BookingCreateDto dto);
        Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId);
        Task<IEnumerable<Booking>> GetBookingsByVendorIdAsync(int vendorId);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
        Task<decimal> GetTotalBookingAmountByEventAsync(int eventId);
        Task<IEnumerable<Booking>> GetPendingPaymentsAsync();
        Task<Booking> PartialUpdateAsync(int id, BookingStatus? status, PaymentStatus? paymentStatus, decimal? paidAmount, string serviceName, string serviceDescription, decimal? amount, DateTime? serviceDate, string timeSlot, string paymentMethod, string notes);
    }
}
