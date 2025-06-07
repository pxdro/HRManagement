using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HRManagement.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, ILogger<EmployeeService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResultDto<IEnumerable<EmployeeReturnDto>>> GetAllAsync()
        {
            try
            {
                var employees = await _unitOfWork.Employees.GetAllAsync(query => query.Include(e => e.Department));
                return ResultDto<IEnumerable<EmployeeReturnDto>>.Success(
                    _mapper.Map<IEnumerable<EmployeeReturnDto>>(employees), 
                    HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<IEnumerable<EmployeeReturnDto>>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<EmployeeReturnDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id, query => query.Include(e => e.Department));
                if (employee == null)
                    return ResultDto<EmployeeReturnDto>.Failure("Employee not found", HttpStatusCode.NotFound);
                else
                    return ResultDto<EmployeeReturnDto>.Success(
                        _mapper.Map<EmployeeReturnDto>(employee), 
                        HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<EmployeeReturnDto>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<EmployeeReturnDto>> AddAsync(EmployeeRequestDto employeeRequestDto)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(employeeRequestDto.Password);
                var employee = new Employee(
                    employeeRequestDto.Name, 
                    employeeRequestDto.Email, 
                    hashedPassword,
                    employeeRequestDto.Position, 
                    employeeRequestDto.HireDate, 
                    employeeRequestDto.IsAdmin, 
                    employeeRequestDto.DepartmentId
                );
                await _unitOfWork.Employees.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                return ResultDto<EmployeeReturnDto>.Success(
                    _mapper.Map<EmployeeReturnDto>(employee), 
                    HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<EmployeeReturnDto>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<EmployeeReturnDto>> UpdateAsync(Guid id, EmployeeRequestDto employeeRequestDto)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);

                if (employee == null)
                    return ResultDto<EmployeeReturnDto>.Failure("Employee not found", HttpStatusCode.NotFound);
                
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(employeeRequestDto.Password);
                employee.Update(
                    employeeRequestDto.Name,
                    employeeRequestDto.Email,
                    hashedPassword,
                    employeeRequestDto.Position,
                    employeeRequestDto.HireDate,
                    employeeRequestDto.IsAdmin,
                    employeeRequestDto.DepartmentId
                );
                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                return ResultDto<EmployeeReturnDto>.Success(
                    _mapper.Map<EmployeeReturnDto>(employee), 
                    HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server error");
                return ResultDto<EmployeeReturnDto>.Failure(
                    "Server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ResultDto<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);

                if (employee == null)
                    return ResultDto<bool>.Failure("Employee not found", HttpStatusCode.NotFound);

                _unitOfWork.Employees.Delete(employee);
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
