using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planner.Application.DTOs
{
    public class BookingCreateDto
    {
        [Required]
        public string ServiceName { get; set; }

        public string? ServiceDescription { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public decimal PaidAmount { get; set; }

        [Required]
        public DateTime ServiceDate { get; set; }

        public string? TimeSlot { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Notes { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int VendorId { get; set; }
    }
}
