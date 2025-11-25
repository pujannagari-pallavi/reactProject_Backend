using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Constants;


namespace Wedding_Planner.API.Controllers
{
    [Route(DashboardRoutes.Base)]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet(DashboardRoutes.Admin)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var result = await _dashboardService.GetAdminDashboardAsync();
            return Ok(result);
        }

        [HttpPost(DashboardRoutes.Client)]
        [Authorize(Roles = Roles.Client)]
        public async Task<IActionResult> ClientDashboard([FromBody] UserIdRequestDto request)
        {
            var result = await _dashboardService.GetClientDashboardAsync(request.UserId);
            return Ok(result);
        }

        [HttpPost(DashboardRoutes.Vendor)]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> GetVendorDashboard([FromBody] UserIdRequestDto request)
        {
            var result = await _dashboardService.GetVendorDashboardAsync(request.UserId);
            return Ok(result);
        }

        [HttpGet(DashboardRoutes.Stats)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetStats()
        {
            var result = await _dashboardService.GetStatsAsync();
            return Ok(result);
        }

        [HttpPost(DashboardRoutes.UserStats)]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUserStats([FromBody] UserIdRequestDto request)
        {
            var result = await _dashboardService.GetUserStatsAsync(request.UserId);
            return Ok(result);
        }

        [HttpGet("analytics")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAnalyticsData()
        {
            var result = await _dashboardService.GetAnalyticsDataAsync();
            return Ok(result);
        }
    }
}
