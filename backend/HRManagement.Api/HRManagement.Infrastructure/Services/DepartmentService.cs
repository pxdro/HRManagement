using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;

namespace HRManagement.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        public Task<ResultDto<DepartmentDto>> AddAsync(DepartmentDto departmentDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<bool>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<IEnumerable<DepartmentDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<DepartmentDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<DepartmentDto>> UpdateAsync(Guid id, DepartmentDto departmentDto)
        {
            throw new NotImplementedException();
        }
    }
}
