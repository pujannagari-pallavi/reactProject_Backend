using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class EventUpdateDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public EventType? EventType { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public string? TimeSlot { get; set; }
        public string? Venue { get; set; }
        public string? City { get; set; }
        public string? GuestRange { get; set; }
        public string? BudgetRange { get; set; }
        public decimal? ActualBudget { get; set; }
        public decimal? SpentAmount { get; set; }
        public EventStatus? Status { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? EventImages { get; set; }
        public int UserId { get; set; }
    }
}
