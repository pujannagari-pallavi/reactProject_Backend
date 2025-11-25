using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Domain.Entities
{
    public class Vendor
    {
        public int Id { get; set; }

        [Required]
        public string BusinessName { get; set; }

        public string Category { get; set; }

        public string ContactPerson { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Services { get; set; }

        public string PriceRange { get; set; }

        public decimal Rating { get; set; }

        public int ReviewCount { get; set; }

        public string LogoUrl { get; set; }

        public string GalleryImages { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
