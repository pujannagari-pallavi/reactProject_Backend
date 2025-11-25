using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planner.Application.DTOs
{
    public class ReviewCreateDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int VendorId { get; set; }

        public int? EventId { get; set; }
    }
}
