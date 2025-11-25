using Microsoft.EntityFrameworkCore;
using Wedding_Planner.Data.Data;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(WeddingPlannerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId)
        {
            return await _dbSet.Where(b => b.EventId == eventId).Include(b => b.Vendor).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByVendorIdAsync(int vendorId)
        {
            return await _dbSet.Where(b => b.VendorId == vendorId).Include(b => b.Event).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(b => b.Event)
                .Include(b => b.Vendor)
                .Where(b => b.Event.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
        {
            return await _dbSet.Where(b => b.Status == status).ToListAsync();
        }

        public async Task<decimal> GetTotalBookingAmountByEventAsync(int eventId)
        {
            return await _dbSet.Where(b => b.EventId == eventId).SumAsync(b => b.Amount);
        }

        public async Task<IEnumerable<Booking>> GetPendingPaymentsAsync()
        {
            return await _dbSet.Where(b => b.PaymentStatus == PaymentStatus.Pending || b.PaymentStatus == PaymentStatus.Partial).ToListAsync();
        }
    }
}
