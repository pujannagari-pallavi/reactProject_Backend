using System;
using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public NotificationType Type { get; set; }

        public NotificationPriority Priority { get; set; } = NotificationPriority.Medium;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
    }

    public enum NotificationType
    {
        General = 1,
        Booking = 2,
        Payment = 3,
        Task = 4,
        Event = 5,
        System = 6
    }

    public enum NotificationPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
