using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace HRManagement.Tests.Domain
{
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _service;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ILogger<EmployeeService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public EmployeeServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();

            _loggerMock = new Mock<ILogger<EmployeeService>>();

            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(x => x.Map<IEnumerable<EmployeeReturnDto>>(It.IsAny<IEnumerable<Employee>>()))
                .Returns((IEnumerable<Employee> source) =>
                    source.Select(d => new EmployeeReturnDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Email = d.Email,
                        Position = d.Email,
                        HireDate = d.HireDate,
                        IsAdmin = d.IsAdmin,
                        DepartmentId = d.DepartmentId,
                        Department = new DepartmentReturnDto { Id = d.Department.Id, Name = d.Department.Name, Description = d.Department.Description }
                    }));
            _mapperMock.Setup(x => x.Map<EmployeeReturnDto>(It.IsAny<Employee>()))
                .Returns((Employee source) => new EmployeeReturnDto
                {
                    Id = source.Id,
                    Name = source.Name,
                    Email = source.Email,
                    Position = source.Email,
                    HireDate = source.HireDate,
                    IsAdmin = source.IsAdmin,
                    DepartmentId = source.DepartmentId,
                    Department = source.Department == null ? null
                        : new DepartmentReturnDto { Id = source.Department.Id, Name = source.Department.Name, Description = source.Department.Description }
                });

            _service = new EmployeeService(_uowMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsSuccessWithEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                CreateEmployee()
            };
            _uowMock.Setup(u => u.Employees.GetAllAsync(It.IsAny<Func<IQueryable<Employee>, IQueryable<Employee>>>()))
                .ReturnsAsync(employees);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.First().Department);
            Assert.Single(result.Data);
            Assert.Null(result.ErrorMessage);
            Assert.Equal("Pedro", result.Data.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_OnException_ReturnsFailure()
        {
            // Valid for all other methods exceptions
            // Arrange
            _uowMock.Setup(u => u.Employees.GetAllAsync(It.IsAny<Func<IQueryable<Employee>, IQueryable<Employee>>>()))
                .ThrowsAsync(new Exception("Db error"));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Null(result.Data);
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal("Server error", result.ErrorMessage);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Server error")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
);
        }

        [Fact]
        public async Task GetByIdAsync_Existing_ReturnsSuccess()
        {
            // Arrange
            var emp = CreateEmployee();

            _uowMock.Setup(u => u.Employees.GetByIdAsync(emp.Id, It.IsAny<Func<IQueryable<Employee>, IQueryable<Employee>>>()))
                .ReturnsAsync(emp);

            // Act
            var result = await _service.GetByIdAsync(emp.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Department);
            Assert.Null(result.ErrorMessage);
            Assert.Equal("Pedro", result.Data.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _uowMock.Setup(u => u.Employees.GetByIdAsync(id, It.IsAny<Func<IQueryable<Employee>, IQueryable<Employee>>>()))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Null(result.Data);
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal("Employee not found", result.ErrorMessage);
        }

        [Fact]
        public async Task AddAsync_Success_ReturnsCreated()
        {
            // Arrange
            var emp = CreateEmployee();
            _uowMock.Setup(u => u.Employees.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var req = new EmployeeRequestDto { 
                Name = "Pedro", 
                Email = "pedro@test.com", 
                Password = "AnyPass123", 
                Position = "Software Engineer", 
                HireDate = DateTime.UtcNow.AddYears(-1), 
                IsAdmin = true, 
                DepartmentId = emp.Department.Id
            };

            // Act
            var result = await _service.AddAsync(req);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Null(result.ErrorMessage);
            Assert.NotNull(result.Data);
            Assert.Equal(req.Name, result.Data.Name);
            Assert.Equal(emp.Name, result.Data.Name);
        }

        [Fact]
        public async Task UpdateAsync_Existing_ReturnsOk()
        {
            // Arrange
            var emp = CreateEmployee();
            _uowMock.Setup(u => u.Employees.GetByIdAsync(emp.Id, null)).ReturnsAsync(emp);
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var req = new EmployeeRequestDto { 
                Name = "Pedro2", 
                Email = "pedro@test.com", 
                Password = "AnyPass123", 
                Position = "Software Engineer", 
                HireDate = DateTime.UtcNow.AddYears(-1), 
                IsAdmin = true, 
                DepartmentId = emp.Department.Id };

            // Act
            var result = await _service.UpdateAsync(emp.Id, req);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.ErrorMessage);
            Assert.NotNull(result.Data);
            Assert.Equal(req.Name, result.Data.Name);
        }

        [Fact]
        public async Task UpdateAsync_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _uowMock.Setup(u => u.Employees.GetByIdAsync(id, null)).ReturnsAsync((Employee)null);

            var req = new EmployeeRequestDto();

            // Act
            var result = await _service.UpdateAsync(id, req);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.ErrorMessage);
            Assert.Null(result.Data);
            Assert.Equal("Employee not found", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteAsync_Existing_ReturnsNoContent()
        {
            // Arrange
            var emp = new Employee("Pedro", "pedro@test.com", "H4sh3DP@ss!", "Software Engineer", DateTime.UtcNow.AddYears(-1), true, Guid.NewGuid());
            _uowMock.Setup(u => u.Employees.GetByIdAsync(emp.Id, null)).ReturnsAsync(emp);
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(emp.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Null(result.ErrorMessage);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _uowMock.Setup(u => u.Employees.GetByIdAsync(id, null)).ReturnsAsync((Employee)null);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.ErrorMessage);
            Assert.False(result.Data);
            Assert.Equal("Employee not found", result.ErrorMessage);
        }
        private Employee CreateEmployee(string name = "Pedro")
        {
            var dept = new Department("IT", "Information Tech");
            return new Employee(name, $"{name.ToLower()}@test.com", "pass", "SE", DateTime.UtcNow.AddYears(-1), true, dept.Id)
            {
                Department = dept
            };
        }
    }
}
