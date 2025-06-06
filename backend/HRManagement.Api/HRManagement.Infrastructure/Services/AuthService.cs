using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HRManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _jwtSecret;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public AuthService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new InvalidOperationException("JWT_SECRET is missing.");
            _logger = logger;
        }

        public async Task<ResultDto<string>> LoginAsync(LoginDto registerDto)
        {
            try
            {
                var users = await _unitOfWork.Employees.GetAllAsync();
                var user = users.Where(u => u.Email == registerDto.Email).FirstOrDefault();
                if (user == null)
                    return ResultDto<string>.Failure("User not found", HttpStatusCode.NotFound);

                if (!BCrypt.Net.BCrypt.Verify(registerDto.Password, user.HashedPassword))
                    return ResultDto<string>.Failure("Invalid credentials", HttpStatusCode.Unauthorized);

                var token = GenerateToken(user);
                return ResultDto<string>.Success(token, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<string>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public string GenerateToken(Employee employee)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.IsAdmin ? "Admin" : "Employee"),
                employee.IsAdmin ? new Claim(ClaimTypes.Role, "Admin") : new Claim(ClaimTypes.Role, "Normal")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
