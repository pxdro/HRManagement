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
        private readonly ILogger _logger;

        public EmployeeService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResultDto<IEnumerable<EmployeeReturnDto>>> GetAllAsync()
        {
            try
            {
                var employees = await _unitOfWork.Employees.GetAllAsync(query => query.Include(e => e.Department));
                var result = employees.Select(e => new EmployeeReturnDto 
                    { Id = e.Id, Name = e.Name, Email = e.Email, Position = e.Position, HireDate = e.HireDate, IsAdmin = e.IsAdmin, DepartmentId = e.DepartmentId,
                      Department = new DepartmentReturnDto { Name = e.Department.Name, Description = e.Department.Description }
                    });
                return ResultDto<IEnumerable<EmployeeReturnDto>>.Success(result, HttpStatusCode.OK);
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
                        new EmployeeReturnDto
                        {
                            Id = employee.Id,
                            Name = employee.Name,
                            Email = employee.Email,
                            Position = employee.Position,
                            HireDate = employee.HireDate,
                            IsAdmin = employee.IsAdmin,
                            DepartmentId = employee.DepartmentId,
                            Department = new DepartmentReturnDto { Name = employee.Department.Name, Description = employee.Department.Description }
                        },
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
                    new EmployeeReturnDto
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Email = employee.Email,
                        Position = employee.Position,
                        HireDate = employee.HireDate,
                        IsAdmin = employee.IsAdmin,
                        DepartmentId = employee.DepartmentId,
                    },
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
                    new EmployeeReturnDto
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Email = employee.Email,
                        Position = employee.Position,
                        HireDate = employee.HireDate,
                        IsAdmin = employee.IsAdmin,
                        DepartmentId = employee.DepartmentId,
                        Department = new DepartmentReturnDto { Name = employee.Department.Name, Description = employee.Department.Description }
                    },
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
