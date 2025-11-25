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
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var vendors = await _vendorService.GetAllAsync();
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var vendor = await _vendorService.GetByIdAsync(id);
            if (vendor == null)
                return NotFound();
            return Ok(vendor);
        }

        [HttpPost("details")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVendorDetails([FromBody] IdRequestDto request)
        {
            var vendor = await _vendorService.GetByIdAsync(request.Id);
            if (vendor == null)
                return NotFound();
            return Ok(vendor);
        }

        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var vendors = await _vendorService.GetVendorsByCategoryAsync(category);
            return Ok(vendors);
        }

        [HttpGet("city/{city}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCity(string city)
        {
            var vendors = await _vendorService.GetVendorsByCityAsync(city);
            return Ok(vendors);
        }

        [HttpGet("verified")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVerified()
        {
            var vendors = await _vendorService.GetVerifiedVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("unverified")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUnverified()
        {
            var vendors = await _vendorService.GetUnverifiedVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("top-rated")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopRated([FromQuery] int count = 10)
        {
            var vendors = await _vendorService.GetTopRatedVendorsAsync(count);
            return Ok(vendors);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var vendors = await _vendorService.SearchVendorsAsync(term);
            return Ok(vendors);
        }

        [HttpPut("update")]
        [Authorize(Roles = Roles.VendorAdmin)]
        public async Task<IActionResult> Update([FromBody] VendorUpdateDto dto)
        {
            var updated = await _vendorService.PartialUpdateAsync(dto.Id, dto.BusinessName, dto.Category, dto.ContactPerson, dto.Email, dto.PhoneNumber, dto.City, dto.Address, dto.Description, dto.Services, dto.PriceRange, dto.GalleryImages, dto.IsActive);
            
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }


        [HttpPut("{id}/verify")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> VerifyVendor(int id)
        {
            var result = await _vendorService.VerifyVendorAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = SuccessMessages.VendorVerified });
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RejectVendor(int id)
        {
            var result = await _vendorService.RejectVendorAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = SuccessMessages.VendorRejected });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _vendorService.DeleteAsync(id);
            return Ok(new { message = SuccessMessages.DeletedSuccessfully });
        }

        [HttpPost("{id}/upload-logo")]
        [Authorize(Roles = Roles.VendorAdmin)]
        public async Task<IActionResult> UploadLogo(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ErrorMessages.NoFileUploaded);

            var url = await _vendorService.UploadLogoAsync(id, file);
            if (url == null)
                return NotFound();

            return Ok(new { url });
        }

        [HttpPost("{id}/upload-gallery")]
        [Authorize(Roles = Roles.VendorAdmin)]
        public async Task<IActionResult> UploadGalleryImages(int id, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest(ErrorMessages.NoFileUploaded);

            var result = await _vendorService.UploadGalleryImagesAsync(id, files);
            if (result.urls == null)
                return NotFound();

            return Ok(new { urls = result.urls, galleryImages = result.galleryImages });
        }

        [HttpDelete("gallery/delete")]
        [Authorize(Roles = Roles.VendorAdmin)]
        public async Task<IActionResult> DeleteGalleryImage([FromBody] DeleteGalleryImageDto request)
        {
            var updated = await _vendorService.RemoveGalleryImageAsync(request.VendorId, request.ImageUrl);
            if (updated == null) return NotFound();

            return Ok(new { message = SuccessMessages.ImageDeleted, galleryImages = updated.GalleryImages });
        }
    }
}
