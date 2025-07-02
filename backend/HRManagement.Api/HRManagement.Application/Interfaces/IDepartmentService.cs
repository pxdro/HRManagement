using HRManagement.Application.DTOs;

namespace HRManagement.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<ResultDto<IEnumerable<DepartmentReturnDto>>> GetAllAsync();
        Task<ResultDto<DepartmentReturnDto>> GetByIdAsync(Guid id);
        Task<ResultDto<DepartmentReturnDto>> AddAsync(DepartmentCreationRequestDto departmentDto);
        Task<ResultDto<DepartmentReturnDto>> UpdateAsync(Guid id, DepartmentUpdateRequestDto departmentDto);
        Task<ResultDto<bool>> DeleteAsync(Guid id);
    }
}
