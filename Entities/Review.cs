using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding_Planner.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public string? ReviewImages { get; set; }

        public bool IsApproved { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int UserId { get; set; }
        public int VendorId { get; set; }
        public int? EventId { get; set; }
        public int? EventPlannerId { get; set; }
        

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual User EventPlanner { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Event Event { get; set; }
    }
}
