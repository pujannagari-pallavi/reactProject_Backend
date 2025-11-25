using Microsoft.EntityFrameworkCore;
using Wedding_Planner.Data.Data;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Implementations
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(WeddingPlannerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByVendorIdAsync(int vendorId)
        {
            return await _dbSet.Where(r => r.VendorId == vendorId && r.IsApproved)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(r => r.UserId == userId)
                .Include(r => r.Vendor)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetApprovedReviewsAsync()
        {
            return await _dbSet.Where(r => r.IsApproved).ToListAsync();
        }

        public async Task<decimal> GetAverageRatingByVendorAsync(int vendorId)
        {
            var reviews = await _dbSet.Where(r => r.VendorId == vendorId && r.IsApproved).ToListAsync();
            return reviews.Any() ? (decimal)reviews.Average(r => r.Rating) : 0;
        }

        public async Task<bool> ApproveReviewAsync(int reviewId)
        {
            var review = await GetByIdAsync(reviewId);
            if (review == null) return false;

            review.IsApproved = true;
            await UpdateAsync(review);
            return true;
        }
    }
}
