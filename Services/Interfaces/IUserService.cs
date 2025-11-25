using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<bool> VerifyEventPlannerAsync(int userId);
        Task<bool> RejectEventPlannerAsync(int userId);
        Task<IEnumerable<User>> GetUnverifiedEventPlannersAsync();
        Task<User> PartialUpdateAsync(int id, string title, string firstName, string lastName, string phoneNumber, string city, string profileImageUrl, bool? isActive, bool? isEmailVerified);
        Task<IEnumerable<User>> GetNonAdminUsersAsync();
        Task<string> UploadProfileImageAsync(int userId, IFormFile file);
    }
}
