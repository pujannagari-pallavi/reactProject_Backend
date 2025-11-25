using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Data.Data;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Implementations
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(WeddingPlannerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByStatusAsync(EventStatus status)
        {
            return await _dbSet.Where(e => e.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            return await _dbSet.Where(e => e.EventDate > DateTime.UtcNow).OrderBy(e => e.EventDate).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(e => e.EventDate >= startDate && e.EventDate <= endDate).ToListAsync();
        }

        public async Task<Event> GetEventWithDetailsAsync(int eventId)
        {
            return await _dbSet
                .Include(e => e.User)
                .Include(e => e.Bookings)
                .Include(e => e.Tasks)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }
    }
}
