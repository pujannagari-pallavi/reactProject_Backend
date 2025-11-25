using System;
using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public string? MessageType { get; set; }

        public MessagePriority? Priority { get; set; } = MessagePriority.Medium;

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        // Foreign Keys
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int? EventId { get; set; }

        // Navigation Properties
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public virtual Event Event { get; set; }
    }

    public enum MessagePriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }
}
