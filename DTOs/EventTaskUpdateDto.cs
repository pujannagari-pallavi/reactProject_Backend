using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class EventTaskUpdateDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority? Priority { get; set; }
        public TaskStatuses? TaskStatus { get; set; }
        public string? AssignedTo { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
