using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Wedding_Planner.Application.DTOs;
using Wedding_Planner.Application.Services.Interfaces;
using Wedding_Planner.Domain.Constants;
using Wedding_Planner.Domain.Entities;

namespace Wedding_Planner.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUserService userService, IVendorService vendorService, IConfiguration configuration, IMapper mapper)
        {
            _userService = userService;
            _vendorService = vendorService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<User> RegisterClientAsync(ClientRegisterDto dto)
        {
            if (await _userService.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("Email already exists");

            var user = _mapper.Map<User>(dto);
            user.Role = UserRole.Client;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            return await _userService.CreateAsync(user);
        }

        public async Task<User> RegisterEventPlannerAsync(EventPlannerRegisterDto dto)
        {
            if (await _userService.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("Email already exists");

            var user = _mapper.Map<User>(dto);
            user.Role = UserRole.EventPlanner;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            return await _userService.CreateAsync(user);
        }

        public async Task<Vendor> RegisterVendorAsync(VendorRegisterDto dto)
        {
            if (await _userService.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("Email already exists");

            var user = _mapper.Map<User>(dto);
            var vendor = _mapper.Map<Vendor>(dto);
            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Role = UserRole.Vendor;
            var createdUser = await _userService.CreateAsync(user);

            // Copy user details to vendor
            vendor.Email = createdUser.Email;
            vendor.PhoneNumber = createdUser.PhoneNumber;
            vendor.City = createdUser.City;
            vendor.ContactPerson = $"{createdUser.FirstName} {createdUser.LastName}";
            vendor.Description = vendor.Description ?? DefaultValues.DefaultVendorDescription;
            vendor.LogoUrl = vendor.LogoUrl ?? DefaultValues.DefaultVendorLogo;
            
            // Convert services to JSON array format if not already
            if (!string.IsNullOrEmpty(vendor.Services) && !vendor.Services.StartsWith("["))
            {
                var servicesList = vendor.Services.Split(',').Select(s => $"\"{s.Trim()}\"");
                vendor.Services = $"[{string.Join(",", servicesList)}]";
            }
            else if (string.IsNullOrEmpty(vendor.Services))
            {
                vendor.Services = DefaultValues.EmptyJsonArray;
            }
            
            vendor.GalleryImages = vendor.GalleryImages ?? DefaultValues.EmptyJsonArray;
            vendor.Rating = 0;
            vendor.ReviewCount = 0;
            vendor.IsActive = true;
            vendor.IsVerified = false;

            return await _vendorService.CreateAsync(vendor);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated");

            return user;
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
