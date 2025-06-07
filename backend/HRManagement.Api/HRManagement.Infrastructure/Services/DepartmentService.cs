using AutoMapper;
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
        private readonly ILogger<DepartmentService> _logger;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, ILogger<DepartmentService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResultDto<IEnumerable<DepartmentReturnDto>>> GetAllAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                return ResultDto<IEnumerable<DepartmentReturnDto>>.Success(
                    _mapper.Map<IEnumerable<DepartmentReturnDto>>(departments), 
                    HttpStatusCode.OK);
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
                        _mapper.Map<DepartmentReturnDto>(department), 
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
                    _mapper.Map<DepartmentReturnDto>(department),
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
                    _mapper.Map<DepartmentReturnDto>(department),
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
