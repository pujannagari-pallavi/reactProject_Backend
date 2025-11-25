using Microsoft.EntityFrameworkCore;
using Wedding_Planner.Data.Data;
using Wedding_Planner.Data.Repositories.Interfaces;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Implementations
{
    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(WeddingPlannerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vendor>> GetVendorsByCategoryAsync(string category)
        {
            return await _dbSet.Where(v => v.Category == category && v.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Vendor>> GetVendorsByCityAsync(string city)
        {
            return await _dbSet.Where(v => v.City == city && v.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Vendor>> GetVerifiedVendorsAsync()
        {
            return await _dbSet.Where(v => v.IsVerified && v.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10)
        {
            return await _dbSet.Where(v => v.IsActive).OrderByDescending(v => v.Rating).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Vendor>> SearchVendorsAsync(string searchTerm)
        {
            return await _dbSet.Where(v => v.IsActive &&
                (v.BusinessName.Contains(searchTerm) || v.Category.Contains(searchTerm) || v.City.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
