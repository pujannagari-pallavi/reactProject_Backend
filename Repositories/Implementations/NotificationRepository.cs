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
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(WeddingPlannerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _dbSet.Where(n => n.UserId == userId && !n.IsRead).OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await GetByIdAsync(notificationId);
            if (notification == null) return false;

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await UpdateAsync(notification);
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _dbSet.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _dbSet.CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}
