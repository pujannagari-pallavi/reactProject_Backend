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
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(WeddingPlannerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetMessagesBySenderAsync(int senderId)
        {
            return await _dbSet.Where(m => m.SenderId == senderId).OrderByDescending(m => m.SentAt).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByReceiverAsync(int receiverId)
        {
            return await _dbSet.Where(m => m.ReceiverId == receiverId).OrderByDescending(m => m.SentAt).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id)
        {
            return await _dbSet.Where(m =>
                (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.SentAt).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId)
        {
            return await _dbSet.Where(m => m.ReceiverId == userId && !m.IsRead).ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int messageId)
        {
            var message = await GetByIdAsync(messageId);
            if (message == null) return false;

            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
            await UpdateAsync(message);
            return true;
        }
    }
}
