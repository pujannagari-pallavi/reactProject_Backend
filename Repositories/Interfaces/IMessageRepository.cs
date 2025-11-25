using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesBySenderAsync(int senderId);
        Task<IEnumerable<Message>> GetMessagesByReceiverAsync(int receiverId);
        Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id);
        Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId);
        Task<bool> MarkAsReadAsync(int messageId);
    }
}
