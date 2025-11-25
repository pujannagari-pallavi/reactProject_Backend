using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IVendorService : IGenericService<Vendor>
    {
        Task<IEnumerable<Vendor>> GetVendorsByCategoryAsync(string category);
        Task<IEnumerable<Vendor>> GetVendorsByCityAsync(string city);
        Task<IEnumerable<Vendor>> GetVerifiedVendorsAsync();
        Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10);
        Task<IEnumerable<Vendor>> SearchVendorsAsync(string searchTerm);
        Task<bool> VerifyVendorAsync(int vendorId);
        Task<bool> RejectVendorAsync(int vendorId);
        Task<Vendor> PartialUpdateAsync(int id, string businessName, string category, string contactPerson, string email, string phoneNumber, string city, string address, string description, string services, string priceRange, string galleryImages, bool? isActive);
        Task<Vendor> AddGalleryImagesAsync(int id, List<string> imageUrls);
        Task<Vendor> RemoveGalleryImageAsync(int id, string imageUrl);
        Task<IEnumerable<Vendor>> GetUnverifiedVendorsAsync();
        Task<string> UploadLogoAsync(int vendorId, IFormFile file);
        Task<(List<string> urls, string galleryImages)> UploadGalleryImagesAsync(int vendorId, List<IFormFile> files);
    }
}
