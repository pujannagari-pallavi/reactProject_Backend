using System;
using System.Threading.Tasks;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public ContactService(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task SubmitContactFormAsync(string name, string email, string subject, string message)
        {
            var adminUsers = await _userService.GetUsersByRoleAsync(UserRole.Admin);

            foreach (var admin in adminUsers)
            {
                await _notificationService.CreateAsync(new Notification
                {
                    Title = $"Contact Form: {subject}",
                    Message = $"From: {name} ({email})\n\n{message}",
                    UserId = admin.Id,
                    Type = NotificationType.System,
                    Priority = NotificationPriority.High
                });
            }
        }
    }
}
