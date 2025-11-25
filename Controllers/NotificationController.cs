using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Constants;

namespace Wedding_Planner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _notificationService.GetByIdAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        [HttpPost("user-notifications")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetByUser([FromBody] UserIdRequestDto request)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(request.UserId);
            return Ok(notifications);
        }

        [HttpGet("unread")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUnread()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPost("GetUnreadByUser")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUnreadByUser([FromBody] UserIdRequestDto request)
        {
            var notifications = await _notificationService.GetUnreadNotificationsAsync(request.UserId);
            return Ok(notifications);
        }

        [HttpPost("GetUnreadCount")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUnreadCount([FromBody] UserIdRequestDto request)
        {
            var count = await _notificationService.GetUnreadCountAsync(request.UserId);
            return Ok(new { count });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.EventPlanner)]
        public async Task<IActionResult> Create([FromBody] NotificationCreateDto dto)
        {
            var created = await _notificationService.CreateFromDtoAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}/read")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            if (!result)
                return NotFound();
            var notification = await _notificationService.GetByIdAsync(id);
            return Ok(notification);
        }

        [HttpPut("read-all")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return Ok();
        }

        [HttpPost("MarkAllAsReadByUser")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> MarkAllAsReadByUser([FromBody] UserIdRequestDto request)
        {
            var result = await _notificationService.MarkAllAsReadAsync(request.UserId);
            return Ok(new { message = SuccessMessages.AllNotificationsRead });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteAsync(id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }
    }
}
