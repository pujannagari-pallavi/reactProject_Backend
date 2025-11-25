using System.Threading.Tasks;
using Wedding_Planner.Application.DTOs;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<object> GetAdminDashboardAsync();
        Task<ClientDashboardDto> GetClientDashboardAsync(int userId);
        Task<object> GetVendorDashboardAsync(int userId);
        Task<object> GetStatsAsync();
        Task<object> GetUserStatsAsync(int userId);
        Task<object> GetAnalyticsDataAsync();
    }
}
