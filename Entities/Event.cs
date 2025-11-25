using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;

namespace Wedding_Planner.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public EventType EventType { get; set; } // Wedding, Birthday, Corporate, Anniversary

        public string Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public string TimeSlot { get; set; } // Morning, Afternoon, Evening, Night

        public string Venue { get; set; }

        public string City { get; set; }

        public string GuestRange { get; set; } // 1-50, 51-100, 101-200, 201-500, 500+

        public string BudgetRange { get; set; } // 50K-1L, 1L-2L, 2L-5L, 5L+

        public decimal ActualBudget { get; set; }

        public decimal SpentAmount { get; set; }

        public decimal RemainingBudget => ActualBudget - SpentAmount;

        public EventStatus Status { get; set; } = EventStatus.Planning;

        public string CoverImageUrl { get; set; }

        public string EventImages { get; set; } // JSON array of image URLs

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<EventTask> Tasks { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }

    public enum EventStatus
    {
        Planning = 1,
        Confirmed = 2,
        InProgress = 3,
        Completed = 4,
        Cancelled = 5
    }
    public enum EventType
    {
        Wedding=1,
        BirthdayParty=2,
        Corporate=3,
        Anniversary=4
    }
}
