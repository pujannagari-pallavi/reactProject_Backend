using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class MessageService : GenericService<Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        private readonly IBookingService _bookingService;
        private readonly IEventService _eventService;

        public MessageService(IMessageRepository messageRepository, IUserService userService, IVendorService vendorService, IBookingService bookingService, IEventService eventService) : base(messageRepository)
        {
            _messageRepository = messageRepository;
            _userService = userService;
            _vendorService = vendorService;
            _bookingService = bookingService;
            _eventService = eventService;
        }

        public async Task<IEnumerable<Message>> GetMessagesBySenderAsync(int senderId)
        {
            return await _messageRepository.GetMessagesBySenderAsync(senderId);
        }

        public async Task<IEnumerable<Message>> GetMessagesByReceiverAsync(int receiverId)
        {
            return await _messageRepository.GetMessagesByReceiverAsync(receiverId);
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id)
        {
            return await _messageRepository.GetConversationAsync(user1Id, user2Id);
        }

        public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId)
        {
            return await _messageRepository.GetUnreadMessagesAsync(userId);
        }

        public async Task<bool> MarkAsReadAsync(int messageId)
        {
            return await _messageRepository.MarkAsReadAsync(messageId);
        }

        public override async Task<Message> CreateAsync(Message message)
        {
            message.SentAt = DateTime.UtcNow;
            return await base.CreateAsync(message);
        }

        public async Task<Message> CreateMessageFromDtoAsync(int senderId, int receiverId, string content, string subject, int priority)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content ?? string.Empty,
                Subject = string.IsNullOrEmpty(subject) ? "No Subject" : subject,
                Priority = Enum.IsDefined(typeof(MessagePriority), priority)
                    ? (MessagePriority)priority
                    : MessagePriority.Medium,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            return await CreateAsync(message);
        }

        public async Task<IEnumerable<User>> GetUserContactsAsync(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return new List<User>();

            var contacts = new List<User>();

            if (user.Role == UserRole.Client)
            {
                var userEvents = await _eventService.GetEventsByUserIdAsync(userId);
                var vendorIds = new HashSet<int>();

                foreach (var evt in userEvents)
                {
                    var eventBookings = await _bookingService.GetBookingsByEventIdAsync(evt.Id);
                    foreach (var booking in eventBookings)
                    {
                        vendorIds.Add(booking.VendorId);
                    }
                }

                var allVendorUsers = await _userService.GetUsersByRoleAsync(UserRole.Vendor);
                var allVendors = await _vendorService.GetAllAsync();

                foreach (var vendorId in vendorIds)
                {
                    var vendor = allVendors.FirstOrDefault(v => v.Id == vendorId);
                    if (vendor != null)
                    {
                        var vendorUser = allVendorUsers.FirstOrDefault(u => u.Email == vendor.Email);
                        if (vendorUser != null && !contacts.Any(c => c.Id == vendorUser.Id))
                        {
                            contacts.Add(vendorUser);
                        }
                    }
                }
            }
            else if (user.Role == UserRole.Vendor)
            {
                var allVendors = await _vendorService.GetAllAsync();
                var vendor = allVendors.FirstOrDefault(v => v.Email == user.Email);

                if (vendor != null)
                {
                    var vendorBookings = await _bookingService.GetBookingsByVendorIdAsync(vendor.Id);
                    var clientUserIds = new HashSet<int>();

                    foreach (var booking in vendorBookings)
                    {
                        if (booking.Event?.UserId != null)
                        {
                            clientUserIds.Add(booking.Event.UserId);
                        }
                    }

                    foreach (var clientUserId in clientUserIds)
                    {
                        var clientUser = await _userService.GetByIdAsync(clientUserId);
                        if (clientUser != null) contacts.Add(clientUser);
                    }
                }
            }

            var admins = await _userService.GetUsersByRoleAsync(UserRole.Admin);
            if (admins.Any()) contacts.Add(admins.First());

            return contacts.DistinctBy(c => c.Id);
        }
    }
}
