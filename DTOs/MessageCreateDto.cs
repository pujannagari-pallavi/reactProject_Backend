using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planner.Application.DTOs
{
    public class MessageCreateDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        public string Subject { get; set; }

        public int Priority { get; set; } = 1;
    }
}
