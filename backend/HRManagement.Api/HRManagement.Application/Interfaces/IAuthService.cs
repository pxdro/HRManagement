using HRManagement.Application.DTOs;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultDto<string>> LoginAsync(LoginDto registerDto);
        string GenerateToken(Employee employee);
    }
}
