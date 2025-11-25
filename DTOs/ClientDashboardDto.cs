using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class ClientDashboardDto
    {
        public List<Event> Events { get; set; } = new List<Event>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public int UnreadCount { get; set; }
        public List<Vendor> Vendors { get; set; } = new List<Vendor>();
    }
}