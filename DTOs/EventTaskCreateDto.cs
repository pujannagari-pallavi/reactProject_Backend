using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class EventTaskCreateDto
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public string? AssignedTo { get; set; }

        [Required]
        public int EventId { get; set; }
    }
}
