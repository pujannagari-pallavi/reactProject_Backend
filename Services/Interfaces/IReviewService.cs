using System.Collections.Generic;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IReviewService : IGenericService<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByVendorIdAsync(int vendorId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
        Task<IEnumerable<Review>> GetApprovedReviewsAsync();
        Task<decimal> GetAverageRatingByVendorAsync(int vendorId);
        Task<bool> ApproveReviewAsync(int reviewId);
    }
}
