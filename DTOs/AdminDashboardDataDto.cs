using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.DTOs
{
    public class AdminDashboardDataDto
    {
        public int TotalEvents { get; set; }
        public int ActiveBookings { get; set; }
        public int CompletedEvents { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalVendors { get; set; }
        public int VerifiedVendors { get; set; }
        public IEnumerable<Notification> Notifications { get; set; } = new List<Notification>();
        public int UnreadCount { get; set; }
    }
}