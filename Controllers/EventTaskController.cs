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
    public class EventTaskController : ControllerBase
    {
        private readonly IEventTaskService _eventTaskService;

        public EventTaskController(IEventTaskService eventTaskService)
        {
            _eventTaskService = eventTaskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _eventTaskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet(EventTaskRoutes.GetById)]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _eventTaskService.GetByIdAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPost("event-tasks")]
        public async Task<IActionResult> GetByEvent([FromBody] IdRequestDto dto)
        {
            var tasks = await _eventTaskService.GetTasksByEventIdAsync(dto.Id);
            return Ok(tasks);
        }

        [HttpGet(EventTaskRoutes.GetByStatus)]
        public async Task<IActionResult> GetByStatus(int status)
        {
            var tasks = await _eventTaskService.GetTasksByStatusAsync(status);
            return Ok(tasks);
        }

        [HttpGet(EventTaskRoutes.GetPending)]
        public async Task<IActionResult> GetPending()
        {
            var tasks = await _eventTaskService.GetPendingTasksAsync();
            return Ok(tasks);
        }

        [HttpGet(EventTaskRoutes.GetByPriority)]
        public async Task<IActionResult> GetByPriority(TaskPriority priority)
        {
            var tasks = await _eventTaskService.GetTasksByPriorityAsync(priority);
            return Ok(tasks);
        }

        [HttpGet(EventTaskRoutes.GetOverdue)]
        public async Task<IActionResult> GetOverdue()
        {
            var tasks = await _eventTaskService.GetOverdueTasksAsync();
            return Ok(tasks);
        }

        [HttpGet(EventTaskRoutes.DueSoon)]
        public async Task<IActionResult> GetDueSoon([FromQuery] int days = 7)
        {
            var tasks = await _eventTaskService.GetTasksDueSoonAsync(days);
            return Ok(tasks);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ClientEventPlanner)]
        public async Task<IActionResult> Create([FromBody] EventTaskCreateDto dto)
        {
            try
            {
                var created = await _eventTaskService.CreateTaskFromDtoAsync(dto.Title, dto.Description, dto.DueDate, dto.Priority, dto.AssignedTo, dto.EventId);
                return Ok(new { success = true, data = created });
            }
            catch (ArgumentException ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? "";
                var fullMessage = $"{ex.Message} | Inner: {innerMessage}";
                Console.WriteLine($"EventTask Create Error: {fullMessage}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                
                if (innerMessage.Contains("FK_EventTasks_Events") || innerMessage.Contains("FOREIGN KEY constraint"))
                {
                    return Ok(new { success = false, message = "Event not found. Please refresh and try again." });
                }
                return Ok(new { success = false, message = innerMessage.Length > 0 ? innerMessage : ex.Message });
            }
        }

        [HttpPut(EventTaskRoutes.GetById)]
        [Authorize(Roles = Roles.ClientEventPlanner)]
        public async Task<IActionResult> Update(int id, [FromBody] EventTaskUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var updated = await _eventTaskService.PartialUpdateAsync(id, dto.Title, dto.Description, dto.DueDate, dto.Priority, dto.TaskStatus, dto.AssignedTo, dto.IsCompleted);
            
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = Roles.ClientEventPlanner)]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var result = await _eventTaskService.MarkTaskAsCompletedAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { message = EventTaskMessages.Completed });
        }

        [HttpDelete("delete-task")]
        [Authorize(Roles = Roles.ClientEventPlannerAdmin)]
        public async Task<IActionResult> Delete([FromBody] IdRequestDto dto)
        {
            await _eventTaskService.DeleteAsync(dto.Id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }
    }
}
