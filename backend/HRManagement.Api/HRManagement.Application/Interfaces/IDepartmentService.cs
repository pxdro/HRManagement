using HRManagement.Application.DTOs;

namespace HRManagement.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<ResultDto<IEnumerable<DepartmentDto>>> GetAllAsync();
        Task<ResultDto<DepartmentDto>> GetByIdAsync(Guid id);
        Task<ResultDto<DepartmentDto>> AddAsync(DepartmentDto departmentDto);
        Task<ResultDto<DepartmentDto>> UpdateAsync(Guid id, DepartmentDto departmentDto);
        Task<ResultDto<bool>> DeleteAsync(Guid id);
    }
}
