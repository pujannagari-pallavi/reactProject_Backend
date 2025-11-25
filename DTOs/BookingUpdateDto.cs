using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class BookingUpdateDto
    {
        public int Id { get; set; }
        public string? ServiceDescription { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? ServiceName { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string? TimeSlot { get; set; }
        public string? PaymentMethod { get; set; }
        public BookingStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string? Notes { get; set; }
    }
}
