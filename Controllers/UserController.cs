using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Entities;
using Wedding_Planner.Domain.Constants;

namespace Wedding_Planner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("non-admin")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetNonAdminUsers()
        {
            var users = await _userService.GetNonAdminUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUserById([FromQuery] int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUserByIdRoute(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("role/{role}")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> GetUsersByRole(UserRole role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpPut("update")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto dto)
        {
            var updated = await _userService.PartialUpdateAsync(dto.Id, dto.Title, dto.FirstName, dto.LastName, dto.PhoneNumber, dto.City, dto.ProfileImageUrl, dto.IsActive, dto.IsEmailVerified);
            
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }

        [HttpGet("eventplanners/unverified")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUnverifiedEventPlanners()
        {
            var users = await _userService.GetUnverifiedEventPlannersAsync();
            return Ok(users);
        }

        [HttpPut("eventplanners/{id}/verify")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> VerifyEventPlanner(int id)
        {
            var result = await _userService.VerifyEventPlannerAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = SuccessMessages.EventPlannerVerified });
        }

        [HttpPut("eventplanners/{id}/reject")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RejectEventPlanner(int id)
        {
            var result = await _userService.RejectEventPlannerAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = SuccessMessages.EventPlannerRejected });
        }

        [HttpPost("{id}/upload-profile-image")]
        [Authorize(Roles = Roles.All)]
        public async Task<IActionResult> UploadProfileImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ErrorMessages.NoFileUploaded);

            var url = await _userService.UploadProfileImageAsync(id, file);
            if (url == null)
                return NotFound();

            return Ok(new { url });
        }


    }
}
