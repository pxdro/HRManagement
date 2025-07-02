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
                var employees = await _unitOfWork.Employees.GetAllAsync(query => query.AsNoTracking().Include(e => e.Department));
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
                var employee = await _unitOfWork.Employees.GetByIdAsync(id, query => query.AsNoTracking().Include(e => e.Department));
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

        public async Task<ResultDto<EmployeeReturnDto>> AddAsync(EmployeeCreationRequestDto employeeRequestDto)
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
                await _unitOfWork.CompleteAsync();

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

        public async Task<ResultDto<EmployeeReturnDto>> UpdateAsync(Guid id, EmployeeUpdateRequestDto dto)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id);

                if (employee == null)
                    return ResultDto<EmployeeReturnDto>.Failure("Employee not found", HttpStatusCode.NotFound);

                employee.RowVersion = Convert.FromBase64String(dto.RowVersion);

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                employee.Update(
                    dto.Name,
                    dto.Email,
                    hashedPassword,
                    dto.Position,
                    dto.HireDate,
                    dto.IsAdmin,
                    dto.DepartmentId
                );

                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.CompleteAsync();

                return ResultDto<EmployeeReturnDto>.Success(
                    _mapper.Map<EmployeeReturnDto>(employee),
                    HttpStatusCode.OK
                );
            }
            catch (DbUpdateConcurrencyException)
            {
                return ResultDto<EmployeeReturnDto>.Failure(
                    "The record was modified by another user",
                    HttpStatusCode.Conflict
                );
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
                await _unitOfWork.CompleteAsync();

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
