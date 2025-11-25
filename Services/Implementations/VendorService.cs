using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class VendorService : GenericService<Vendor>, IVendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository) : base(vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<IEnumerable<Vendor>> GetVendorsByCategoryAsync(string category)
        {
            return await _vendorRepository.GetVendorsByCategoryAsync(category);
        }

        public async Task<IEnumerable<Vendor>> GetVendorsByCityAsync(string city)
        {
            return await _vendorRepository.GetVendorsByCityAsync(city);
        }

        public async Task<IEnumerable<Vendor>> GetVerifiedVendorsAsync()
        {
            return await _vendorRepository.GetVerifiedVendorsAsync();
        }

        public async Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10)
        {
            return await _vendorRepository.GetTopRatedVendorsAsync(count);
        }

        public async Task<IEnumerable<Vendor>> SearchVendorsAsync(string searchTerm)
        {
            return await _vendorRepository.SearchVendorsAsync(searchTerm);
        }

        public override async Task<Vendor> CreateAsync(Vendor vendor)
        {
            vendor.CreatedAt = DateTime.UtcNow;
            vendor.IsActive = true;
            vendor.IsVerified = false;
            return await base.CreateAsync(vendor);
        }

        public override async Task<Vendor> UpdateAsync(Vendor vendor)
        {
            vendor.UpdatedAt = DateTime.UtcNow;
            return await base.UpdateAsync(vendor);
        }

        public async Task<Vendor> PartialUpdateAsync(int id, string businessName, string category, string contactPerson, string email, string phoneNumber, string city, string address, string description, string services, string priceRange, string galleryImages, bool? isActive)
        {
            var existingVendor = await _vendorRepository.GetByIdAsync(id);
            if (existingVendor == null) return null;

            if (!string.IsNullOrEmpty(businessName)) existingVendor.BusinessName = businessName;
            if (!string.IsNullOrEmpty(category)) existingVendor.Category = category;
            if (!string.IsNullOrEmpty(contactPerson)) existingVendor.ContactPerson = contactPerson;
            if (!string.IsNullOrEmpty(email)) existingVendor.Email = email;
            if (!string.IsNullOrEmpty(phoneNumber)) existingVendor.PhoneNumber = phoneNumber;
            if (!string.IsNullOrEmpty(city)) existingVendor.City = city;
            if (!string.IsNullOrEmpty(address)) existingVendor.Address = address;
            if (!string.IsNullOrEmpty(description)) existingVendor.Description = description;
            if (!string.IsNullOrEmpty(services)) existingVendor.Services = services;
            if (!string.IsNullOrEmpty(priceRange)) existingVendor.PriceRange = priceRange;
            if (!string.IsNullOrEmpty(galleryImages)) existingVendor.GalleryImages = galleryImages;
            if (isActive.HasValue) existingVendor.IsActive = isActive.Value;

            return await UpdateAsync(existingVendor);
        }

        public async Task<Vendor> AddGalleryImagesAsync(int id, List<string> imageUrls)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);
            if (vendor == null) return null;

            var currentGallery = new List<string>();
            if (!string.IsNullOrEmpty(vendor.GalleryImages) && vendor.GalleryImages != "[]")
            {
                try
                {
                    currentGallery = System.Text.Json.JsonSerializer.Deserialize<List<string>>(vendor.GalleryImages) ?? new List<string>();
                }
                catch { }
            }

            currentGallery.AddRange(imageUrls);
            vendor.GalleryImages = System.Text.Json.JsonSerializer.Serialize(currentGallery);
            return await UpdateAsync(vendor);
        }

        public async Task<Vendor> RemoveGalleryImageAsync(int id, string imageUrl)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);
            if (vendor == null) return null;

            var currentGallery = new List<string>();
            if (!string.IsNullOrEmpty(vendor.GalleryImages) && vendor.GalleryImages != "[]")
            {
                try
                {
                    currentGallery = System.Text.Json.JsonSerializer.Deserialize<List<string>>(vendor.GalleryImages) ?? new List<string>();
                }
                catch { }
            }

            currentGallery.Remove(imageUrl);
            vendor.GalleryImages = System.Text.Json.JsonSerializer.Serialize(currentGallery);
            return await UpdateAsync(vendor);
        }
        public async Task<bool> VerifyVendorAsync(int vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null) return false;

            vendor.IsVerified = true;
            vendor.UpdatedAt = DateTime.UtcNow;
            await _vendorRepository.UpdateAsync(vendor);
            return true;
        }

        public async Task<bool> RejectVendorAsync(int vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null) return false;

            vendor.IsVerified = false;
            vendor.UpdatedAt = DateTime.UtcNow;
            await _vendorRepository.UpdateAsync(vendor);
            return true;
        }

        public async Task<IEnumerable<Vendor>> GetUnverifiedVendorsAsync()
        {
            var vendors = await _vendorRepository.GetAllAsync();
            return vendors.Where(v => !v.IsVerified);
        }

        public async Task<string> UploadLogoAsync(int vendorId, IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "vendors");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null) return null;

            vendor.LogoUrl = $"/uploads/vendors/{fileName}";
            await UpdateAsync(vendor);

            return vendor.LogoUrl;
        }

        public async Task<(List<string> urls, string galleryImages)> UploadGalleryImagesAsync(int vendorId, List<IFormFile> files)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null) return (null, null);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "gallery");
            Directory.CreateDirectory(uploadsFolder);

            var uploadedUrls = new List<string>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedUrls.Add($"/uploads/gallery/{fileName}");
                }
            }

            var updated = await AddGalleryImagesAsync(vendorId, uploadedUrls);
            return (uploadedUrls, updated.GalleryImages);
        }
    }
}
