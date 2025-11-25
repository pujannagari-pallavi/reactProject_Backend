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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ReviewController(IReviewService reviewService, IMapper mapper, INotificationService notificationService)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        [HttpPost("vendor-reviews")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByVendor([FromBody] VendorIdRequestDto request)
        {
            var reviews = await _reviewService.GetReviewsByVendorIdAsync(request.VendorId);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = Roles.Client + "," + Roles.Admin)]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }

        [HttpGet("approved")]
        [AllowAnonymous]
        public async Task<IActionResult> GetApproved()
        {
            var reviews = await _reviewService.GetApprovedReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("average-rating/{vendorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(int vendorId)
        {
            var rating = await _reviewService.GetAverageRatingByVendorAsync(vendorId);
            return Ok(new { averageRating = rating });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Client)]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
        {
            var review = _mapper.Map<Review>(dto);
            var created = await _reviewService.CreateAsync(review);
            return Ok(created);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _reviewService.ApproveReviewAsync(id);
            if (!result)
                return NotFound();
            
            return Ok(new { message = SuccessMessages.ReviewApproved });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _reviewService.DeleteAsync(id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }
    }
}
