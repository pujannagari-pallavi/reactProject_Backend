namespace Wedding_Planner.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalEvents { get; set; }
        public int ActiveBookings { get; set; }
        public int CompletedEvents { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingTasks { get; set; }
        public int TotalVendors { get; set; }
        public int VerifiedVendors { get; set; }
        public List<MonthlyStatsDto> MonthlyStats { get; set; } = new();
    }

    public class MonthlyStatsDto
    {
        public string Month { get; set; }
        public int EventCount { get; set; }
        public decimal Revenue { get; set; }
    }
}