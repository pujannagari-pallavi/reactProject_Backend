using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Application.DTOs
{
    public class ContactDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}