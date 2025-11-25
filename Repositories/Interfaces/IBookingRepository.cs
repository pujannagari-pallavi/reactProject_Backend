using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId);
        Task<IEnumerable<Booking>> GetBookingsByVendorIdAsync(int vendorId);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
        Task<decimal> GetTotalBookingAmountByEventAsync(int eventId);
        Task<IEnumerable<Booking>> GetPendingPaymentsAsync();
    }
}
