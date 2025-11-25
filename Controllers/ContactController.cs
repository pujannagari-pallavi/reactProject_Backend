using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Constants;

namespace Wedding_Planner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactFormDto dto)
        {
            try
            {
                await _contactService.SubmitContactFormAsync(dto.Name, dto.Email, dto.Subject, dto.Message);
                return Ok(new { message = SuccessMessages.ContactFormSubmitted });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorMessages.ContactFormProcessFailed}: {ex.Message}");
                return StatusCode(500, new { message = ErrorMessages.ContactFormFailed });
            }
        }
    }

    public class ContactFormDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
