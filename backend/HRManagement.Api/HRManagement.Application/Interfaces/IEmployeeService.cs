using HRManagement.Application.DTOs;

namespace HRManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<ResultDto<IEnumerable<EmployeeReturnDto>>> GetAllAsync();
        Task<ResultDto<EmployeeReturnDto>> GetByIdAsync(Guid id);
        Task<ResultDto<EmployeeReturnDto>> AddAsync(EmployeeRequestDto employeeRequestDto);
        Task<ResultDto<EmployeeReturnDto>> UpdateAsync(Guid id, EmployeeRequestDto employeeRequestDto);
        Task<ResultDto<bool>> DeleteAsync(Guid id);
    }
}
