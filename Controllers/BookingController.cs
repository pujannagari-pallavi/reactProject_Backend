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
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = $"Booking with ID {id} not found." });
            return Ok(booking);
        }

        [HttpPost("event-bookings")]
        [Authorize(Roles = Roles.ClientEventPlannerAdmin)]
        public async Task<IActionResult> GetByEvent([FromBody] IdRequestDto dto)
        {
            var bookings = await _bookingService.GetBookingsByEventIdAsync(dto.Id);
            return Ok(bookings);
        }

        [HttpPost("vendor-bookings")]
        [Authorize(Roles = Roles.VendorAdmin)]
        public async Task<IActionResult> GetByVendor([FromBody] VendorIdRequestDto request)
        {
            var bookings = await _bookingService.GetBookingsByVendorIdAsync(request.VendorId);
            return Ok(bookings);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetByStatus(BookingStatus status)
        {
            var bookings = await _bookingService.GetBookingsByStatusAsync(status);
            return Ok(bookings);
        }

        [HttpGet("pending-payments")]
        [Authorize(Roles = Roles.Admin + "," + Roles.EventPlanner)]
        public async Task<IActionResult> GetPendingPayments()
        {
            var bookings = await _bookingService.GetPendingPaymentsAsync();
            return Ok(bookings);
        }

        [HttpGet("total-amount/{eventId}")]
        [Authorize(Roles = Roles.ClientEventPlannerAdmin)]
        public async Task<IActionResult> GetTotalAmount(int eventId)
        {
            var total = await _bookingService.GetTotalBookingAmountByEventAsync(eventId);
            return Ok(new { totalAmount = total });
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = Roles.ClientEventPlannerAdmin)]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            var userBookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(userBookings);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ClientEventPlanner)]
        public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _bookingService.CreateFromDtoAsync(dto);
            return Ok(created);
        }

        [HttpPut]
        [Authorize(Roles = Roles.ClientPlannerVendor)]
        public async Task<IActionResult> Update([FromBody] BookingUpdateDto dto)
        {
            var updated = await _bookingService.PartialUpdateAsync(dto.Id, dto.Status, dto.PaymentStatus, dto.PaidAmount, dto.ServiceName, dto.ServiceDescription, dto.Amount, dto.ServiceDate, dto.TimeSlot, dto.PaymentMethod, dto.Notes);
            
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("delete-booking")]
        [Authorize(Roles = Roles.ClientEventPlannerAdmin)]
        public async Task<IActionResult> DeleteBooking([FromBody] DeleteRequestDto request)
        {
            var existing = await _bookingService.GetByIdAsync(request.Id);
            if (existing == null)
                return NotFound(new { message = $"Booking not found." });

            await _bookingService.DeleteAsync(request.Id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }
    }
}
