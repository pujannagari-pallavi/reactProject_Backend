using Wedding_Planner.Domain.Entities;
using Wedding_Planner.Application.DTOs;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterClientAsync(ClientRegisterDto dto);
        Task<User> RegisterEventPlannerAsync(EventPlannerRegisterDto dto);
        Task<Vendor> RegisterVendorAsync(VendorRegisterDto dto);
        Task<User> LoginAsync(string email, string password);
        Task<string> GenerateJwtTokenAsync(User user);
    }
}
