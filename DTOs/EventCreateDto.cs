using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class EventCreateDto
    {
        [Required]
        public string Title { get; set; }

        public EventType EventType { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public string? TimeSlot { get; set; }

        public string? Venue { get; set; }

        public string? City { get; set; }

        public string? GuestRange { get; set; }

        public string? BudgetRange { get; set; }

        public decimal ActualBudget { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? EventImages { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
