using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Data.Repositories.Interfaces
{
    public interface IVendorRepository : IGenericRepository<Vendor>
    {
        Task<IEnumerable<Vendor>> GetVendorsByCategoryAsync(string category);
        Task<IEnumerable<Vendor>> GetVendorsByCityAsync(string city);
        Task<IEnumerable<Vendor>> GetVerifiedVendorsAsync();
        Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10);
        Task<IEnumerable<Vendor>> SearchVendorsAsync(string searchTerm);
    }
}
