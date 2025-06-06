using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HRManagement.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public DepartmentService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResultDto<IEnumerable<DepartmentReturnDto>>> GetAllAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                var result = departments.Select(d => new DepartmentReturnDto { Id = d.Id, Name = d.Name, Description = d.Description });
                return ResultDto<IEnumerable<DepartmentReturnDto>>.Success(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<IEnumerable<DepartmentReturnDto>>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<DepartmentReturnDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                    return ResultDto<DepartmentReturnDto>.Failure("Department not found", HttpStatusCode.NotFound);
                else
                    return ResultDto<DepartmentReturnDto>.Success(
                        new DepartmentReturnDto { Id = department.Id, Name = department.Name, Description = department.Description }, 
                        HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<DepartmentReturnDto>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<DepartmentReturnDto>> AddAsync(DepartmentRequestDto departmentDto)
        {
            try
            {
                var department = new Department(departmentDto.Name, departmentDto.Description);
                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();
                return ResultDto<DepartmentReturnDto>.Success(
                    new DepartmentReturnDto { Id = department.Id, Name = department.Name, Description = department.Description },
                    HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<DepartmentReturnDto>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<DepartmentReturnDto>> UpdateAsync(Guid id, DepartmentRequestDto departmentDto)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);

                if (department == null)
                    return ResultDto<DepartmentReturnDto>.Failure("Department not found", HttpStatusCode.NotFound);

                department.Update(departmentDto.Name, departmentDto.Description);
                _unitOfWork.Departments.Update(department);
                await _unitOfWork.SaveChangesAsync();

                return ResultDto<DepartmentReturnDto>.Success(
                    new DepartmentReturnDto { Id = department.Id, Name = department.Name, Description = department.Description },
                    HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<DepartmentReturnDto>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);

                if (department == null)
                    return ResultDto<bool>.Failure("Department not found", HttpStatusCode.NotFound);

                _unitOfWork.Departments.Delete(department);
                await _unitOfWork.SaveChangesAsync();

                return ResultDto<bool>.Success(true, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<bool>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }
    }
}
