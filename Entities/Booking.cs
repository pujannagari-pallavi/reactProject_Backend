using System;
using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public string ServiceName { get; set; }

        public string ServiceDescription { get; set; }

        public decimal Amount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal PendingAmount => Amount - PaidAmount;

        public DateTime ServiceDate { get; set; }

        public string TimeSlot { get; set; }

        public string PaymentMethod { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Foreign Keys
        public int EventId { get; set; }
        public int VendorId { get; set; }

        // Navigation Properties
        public virtual Event Event { get; set; }
        public virtual Vendor Vendor { get; set; }
    }

    public enum BookingStatus
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3,
        Completed = 4
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Partial = 2,
        Paid = 3,
        Refunded = 4
    }

    public enum ServiceType
    {
        Photography = 1,
        Videography = 2,
        Catering = 3,
        Decoration = 4,
        Music = 5,
        Venue = 6,
        Transportation = 7,
        Flowers = 8,
        MakeupAndBeauty = 9,
        WeddingCake = 10,
        Lighting = 11,
        Security = 12
    }
}
