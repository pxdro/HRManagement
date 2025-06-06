using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;

namespace HRManagement.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        public Task<ResultDto<EmployeeReturnDto>> AddAsync(EmployeeRequestDto employeeRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<bool>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<IEnumerable<EmployeeReturnDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<EmployeeReturnDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto<EmployeeReturnDto>> UpdateAsync(Guid id, EmployeeRequestDto employeeRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
