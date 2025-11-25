using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class ReviewService : GenericService<Review>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IVendorRepository _vendorRepository;

        public ReviewService(IReviewRepository reviewRepository, IVendorRepository vendorRepository) : base(reviewRepository)
        {
            _reviewRepository = reviewRepository;
            _vendorRepository = vendorRepository;
        }

        public async Task<IEnumerable<Review>> GetReviewsByVendorIdAsync(int vendorId)
        {
            return await _reviewRepository.GetReviewsByVendorIdAsync(vendorId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _reviewRepository.GetReviewsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Review>> GetApprovedReviewsAsync()
        {
            return await _reviewRepository.GetApprovedReviewsAsync();
        }

        public async Task<decimal> GetAverageRatingByVendorAsync(int vendorId)
        {
            return await _reviewRepository.GetAverageRatingByVendorAsync(vendorId);
        }

        public async Task<bool> ApproveReviewAsync(int reviewId)
        {
            var result = await _reviewRepository.ApproveReviewAsync(reviewId);
            if (result)
            {
                var review = await _reviewRepository.GetByIdAsync(reviewId);
                if (review != null)
                {
                    await UpdateVendorRatingAsync(review.VendorId);
                }
            }
            return result;
        }

        private async Task UpdateVendorRatingAsync(int vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor != null)
            {
                var approvedReviews = await _reviewRepository.GetReviewsByVendorIdAsync(vendorId);
                var approved = approvedReviews.Where(r => r.IsApproved).ToList();
                vendor.ReviewCount = approved.Count;
                vendor.Rating = approved.Any() ? (decimal)approved.Average(r => r.Rating) : 0;
                await _vendorRepository.UpdateAsync(vendor);
            }
        }

        public override async Task<Review> CreateAsync(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            review.IsApproved = true; // Auto-approve reviews
            if (string.IsNullOrEmpty(review.Title))
                review.Title = $"{review.Rating} Star Review";
            if (string.IsNullOrEmpty(review.Comment))
                review.Comment = "No comment provided";
            if (string.IsNullOrEmpty(review.ReviewImages))
                review.ReviewImages = "[]";
            // Clear navigation properties to avoid tracking issues
            review.User = null;
            review.Vendor = null;
            review.Event = null;
            review.EventPlanner = null;
            var created = await base.CreateAsync(review);
            
            // Update vendor rating immediately
            await UpdateVendorRatingAsync(review.VendorId);
            
            return created;
        }
    }
}
