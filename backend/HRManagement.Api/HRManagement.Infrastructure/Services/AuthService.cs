using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<AuthService> logger)
        {
            _jwtSecret = configuration["JWT_SECRET"]
                ?? throw new InvalidOperationException("JWT_SECRET is missing.");
            _issuer = configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is missing.");
            _audience = configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is missing.");
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResultDto<TokensDto>> LoginAsync(LoginDto registerDto)
        {
            try
            {
                var users = await _unitOfWork.Employees.GetAllAsync();
                var user = users.Where(u => u.Email == registerDto.Email).FirstOrDefault();
                if (user == null)
                    return ResultDto<TokensDto>.Failure("User not found", HttpStatusCode.NotFound);

                if (!BCrypt.Net.BCrypt.Verify(registerDto.Password, user.HashedPassword))
                    return ResultDto<TokensDto>.Failure("Invalid credentials", HttpStatusCode.Unauthorized);

                var token = GenerateToken(user);
                return ResultDto<TokensDto>.Success(new TokensDto { AuthToken = token }, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<TokensDto>.Failure(
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
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,                 // ex: "HRManagement.Api"
                audience: _audience,             // ex: "HRManagement.Client"
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
