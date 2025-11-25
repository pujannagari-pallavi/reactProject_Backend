using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class NotificationCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public int UserId { get; set; }

        public NotificationType Type { get; set; }
    }
}
