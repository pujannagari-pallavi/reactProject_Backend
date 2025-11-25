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
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(u => u.Role == role);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _userRepository.GetActiveUsersAsync();
        }

        public override async Task<User> CreateAsync(User user)
        {
            if (await _userRepository.EmailExistsAsync(user.Email))
                throw new InvalidOperationException("Email already exists");

            user.CreatedAt = DateTime.UtcNow;
            return await base.CreateAsync(user);
        }

        public override async Task<User> UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            return await base.UpdateAsync(user);
        }

        public async Task<User> PartialUpdateAsync(int id, string title, string firstName, string lastName, string phoneNumber, string city, string profileImageUrl, bool? isActive, bool? isEmailVerified)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return null;

            if (!string.IsNullOrEmpty(title)) existingUser.Title = title;
            if (!string.IsNullOrEmpty(firstName)) existingUser.FirstName = firstName;
            if (!string.IsNullOrEmpty(lastName)) existingUser.LastName = lastName;
            if (!string.IsNullOrEmpty(phoneNumber)) existingUser.PhoneNumber = phoneNumber;
            if (!string.IsNullOrEmpty(city)) existingUser.City = city;
            if (!string.IsNullOrEmpty(profileImageUrl)) existingUser.ProfileImageUrl = profileImageUrl;
            if (isActive.HasValue) existingUser.IsActive = isActive.Value;
            if (isEmailVerified.HasValue) existingUser.IsEmailVerified = isEmailVerified.Value;

            return await UpdateAsync(existingUser);
        }
        public async Task<bool> VerifyEventPlannerAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Role != UserRole.EventPlanner) return false;

            user.IsEmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> RejectEventPlannerAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Role != UserRole.EventPlanner) return false;

            user.IsEmailVerified = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<IEnumerable<User>> GetUnverifiedEventPlannersAsync()
        {
            return await _userRepository.GetAllAsync()
                .ContinueWith(t => t.Result.Where(u => u.Role == UserRole.EventPlanner && !u.IsEmailVerified));
        }

        public async Task<IEnumerable<User>> GetNonAdminUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Where(u => u.Role != UserRole.Admin);
        }

        public async Task<string> UploadProfileImageAsync(int userId, IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            user.ProfileImageUrl = $"/uploads/profiles/{fileName}";
            await UpdateAsync(user);

            return user.ProfileImageUrl;
        }
    }
}
