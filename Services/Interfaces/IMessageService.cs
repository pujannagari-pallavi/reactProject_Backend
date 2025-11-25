using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IMessageService : IGenericService<Message>
    {
        Task<IEnumerable<Message>> GetMessagesBySenderAsync(int senderId);
        Task<IEnumerable<Message>> GetMessagesByReceiverAsync(int receiverId);
        Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id);
        Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId);
        Task<bool> MarkAsReadAsync(int messageId);
        Task<IEnumerable<User>> GetUserContactsAsync(int userId);
        Task<Message> CreateMessageFromDtoAsync(int senderId, int receiverId, string content, string subject, int priority);
    }
}
