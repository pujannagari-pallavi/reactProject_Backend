using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Entities;
using Wedding_Planner.Domain.Constants;

namespace Wedding_Planner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.EventPlanner)]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventEntity = await _eventService.GetByIdAsync(id);
            if (eventEntity == null)
                return NotFound(ErrorMessages.NotFound);

            return Ok(eventEntity);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetWithDetails(int id)
        {
            var eventEntity = await _eventService.GetEventWithDetailsAsync(id);
            if (eventEntity == null)
                return NotFound(ErrorMessages.NotFound);

            return Ok(eventEntity);
        }

        [HttpPost("user-events")]
        [Authorize(Roles = Roles.ClientEventPlannerAdmin)]
        public async Task<IActionResult> GetByUser([FromBody] UserIdRequestDto request)
        {
            var events = await _eventService.GetEventsByUserIdAsync(request.UserId);
            return Ok(events);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.EventPlanner)]
        public async Task<IActionResult> GetByStatus(EventStatus status)
        {
            var events = await _eventService.GetEventsByStatusAsync(status);
            return Ok(events);
        }

        [HttpGet("upcoming")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUpcoming()
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            return Ok(events);
        }

        [HttpGet("date-range")]
        [Authorize(Roles = Roles.Admin + "," + Roles.EventPlanner)]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var events = await _eventService.GetEventsByDateRangeAsync(startDate, endDate);
            return Ok(events);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ClientEventPlanner)]
        public async Task<IActionResult> Create([FromBody] EventCreateDto dto)
        {
            var eventEntity = _mapper.Map<Event>(dto);
            var created = await _eventService.CreateAsync(eventEntity);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.ClientEventPlanner)]
        public async Task<IActionResult> Update(int id, [FromBody] EventUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var updated = await _eventService.PartialUpdateAsync(id, dto.Title, dto.EventType, dto.Description, dto.EventDate, dto.TimeSlot, dto.Venue, dto.City, dto.GuestRange, dto.BudgetRange, dto.ActualBudget, dto.SpentAmount, dto.Status, dto.CoverImageUrl, dto.EventImages);
            
            if (updated == null)
                return NotFound(ErrorMessages.NotFound);

            return Ok(updated);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = Roles.ClientEventPlannerAdmin)]
        public async Task<IActionResult> Delete([FromBody] IdRequestDto request)
        {
            await _eventService.DeleteAsync(request.Id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }

        [HttpPost("{id}/upload-cover")]
        [Authorize(Roles = Roles.ClientEventPlanner)]
        public async Task<IActionResult> UploadCoverImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ErrorMessages.NoFileUploaded);

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                FilePaths.EventUploadFolder
            );

            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = await _eventService.UploadCoverImageAsync(id, fileName);
            if (url == null)
                return NotFound(ErrorMessages.NotFound);

            return Ok(new { url });
        }
    }
}
