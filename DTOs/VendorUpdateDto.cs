using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planner.Application.DTOs
{
    public class VendorUpdateDto
    {
        public int Id { get; set; }
        public string? BusinessName { get; set; }
        public string Category { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? Services { get; set; }
        public string? PriceRange { get; set; }
        public string? GalleryImages { get; set; }
        public bool? IsActive { get; set; }
    }
}
