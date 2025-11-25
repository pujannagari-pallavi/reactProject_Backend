using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Application.DTOs
{
    public class VendorProfileDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string BusinessName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; }
        
        [StringLength(100)]
        public string? ContactPerson { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public string? Services { get; set; }
        
        [StringLength(50)]
        public string? PriceRange { get; set; }
        
        public string? LogoUrl { get; set; }
        
        public List<string> GalleryImages { get; set; } = new();
        
        public List<string> ServicesList { get; set; } = new();
        
        public decimal Rating { get; set; }
        
        public int ReviewCount { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsVerified { get; set; }
    }
}