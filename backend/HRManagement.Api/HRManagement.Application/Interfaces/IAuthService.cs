using HRManagement.Application.DTOs;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultDto<TokensDto>> LoginAsync(LoginDto registerDto);
        string GenerateToken(Employee employee);
    }
}
