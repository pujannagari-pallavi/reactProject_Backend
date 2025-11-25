using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
    }
}
