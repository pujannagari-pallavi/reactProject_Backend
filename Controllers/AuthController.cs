using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Domain.Constants;

namespace Wedding_Planner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/client")]
        public async Task<ActionResult<AuthResponseDto>> RegisterClient(ClientRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { message = ErrorMessages.ValidationFailed, errors });
            }

            try
            {
                var createdUser = await _authService.RegisterClientAsync(registerDto);
                
                var result = new
                {
                    message = SuccessMessages.RegistrationSuccessful,
                    user = new
                    {
                        id = createdUser.Id,
                        email = createdUser.Email,
                        firstName = createdUser.FirstName,
                        lastName = createdUser.LastName,
                        role = createdUser.Role.ToString()
                    }
                };
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ErrorMessages.RegistrationError });
            }
        }

        [HttpPost("register/eventplanner")]
        public async Task<ActionResult<AuthResponseDto>> RegisterEventPlanner(EventPlannerRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { message = ErrorMessages.ValidationFailed, errors });
            }

            try
            {
                var createdUser = await _authService.RegisterEventPlannerAsync(registerDto);
                
                var result = new
                {
                    message = SuccessMessages.RegistrationSuccessful,
                    user = new
                    {
                        id = createdUser.Id,
                        email = createdUser.Email,
                        firstName = createdUser.FirstName,
                        lastName = createdUser.LastName,
                        role = createdUser.Role.ToString()
                    }
                };
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ErrorMessages.RegistrationError });
            }
        }

        [HttpPost("register/vendor")]
        public async Task<ActionResult<AuthResponseDto>> RegisterVendor(VendorRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { message = ErrorMessages.ValidationFailed, errors });
            }

            try
            {
                var createdVendor = await _authService.RegisterVendorAsync(registerDto);
                
                var result = new
                {
                    message = SuccessMessages.RegistrationSuccessful,
                    user = new
                    {
                        id = createdVendor.Id,
                        email = createdVendor.Email,
                        firstName = createdVendor.ContactPerson,
                        lastName = "",
                        role = "Vendor"
                    }
                };
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ErrorMessages.NoInnerException;
                Console.WriteLine($"Vendor registration error: {ex.Message}");
                Console.WriteLine($"Inner exception: {innerMessage}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = ErrorMessages.RegistrationError, error = ex.Message, innerError = innerMessage });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Ok(new { success = false, message = ErrorMessages.ValidationFailed, errors });
            }

            try
            {
                var user = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
                var token = await _authService.GenerateJwtTokenAsync(user);
                
                return Ok(new
                {
                    success = true,
                    token = token,
                    expires = DateTime.UtcNow.AddDays(7),
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        role = user.Role.ToString()
                    }
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ErrorMessages.LoginError });
            }
        }
    }
}
