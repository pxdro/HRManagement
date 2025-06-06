using System.Net;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace HRManagement.Tests.Domain
{
    public class DepartmentServiceTests
    {
        private readonly DepartmentService _service;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ILogger<DepartmentService>> _loggerMock;

        public DepartmentServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DepartmentService>>();
            _service = new DepartmentService(_uowMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsSuccessWithDepartments()
        {
            // Arrange
            var list = new List<Department> { CreateDepartment("HR"), CreateDepartment("IT") };
            _uowMock.Setup(u => u.Departments.GetAllAsync(null))
                .ReturnsAsync(list);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());
            Assert.Contains(result.Data, d => d.Name == "HR");
        }

        [Fact]
        public async Task GetAllAsync_OnException_ReturnsFailure()
        {
            // Valid for all other methods exceptions
            // Arrange
            _uowMock.Setup(u => u.Departments.GetAllAsync(null))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Equal("Server error", result.ErrorMessage);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Server error")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Existing_ReturnsSuccess()
        {
            // Arrange
            var dept = CreateDepartment("Finance");
            _uowMock.Setup(u => u.Departments.GetByIdAsync(dept.Id, null))
                .ReturnsAsync(dept);

            // Act
            var result = await _service.GetByIdAsync(dept.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(dept.Name, result.Data.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _uowMock.Setup(u => u.Departments.GetByIdAsync(id, null))
                .ReturnsAsync((Department)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Equal("Department not found", result.ErrorMessage);
        }

        [Fact]
        public async Task AddAsync_Success_ReturnsCreated()
        {
            // Arrange
            var req = new DepartmentRequestDto { Name = "Legal", Description = "Legal Dept" };
            _uowMock.Setup(u => u.Departments.AddAsync(It.IsAny<Department>()))
                .Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.AddAsync(req);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(req.Name, result.Data.Name);
        }

        [Fact]
        public async Task AddAsync_OnException_ReturnsFailure()
        {
            // Arrange
            var req = new DepartmentRequestDto { Name = "Legal", Description = "Legal Dept" };
            _uowMock.Setup(u => u.Departments.AddAsync(It.IsAny<Department>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _service.AddAsync(req);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Equal("Server error", result.ErrorMessage);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Server error")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Existing_ReturnsOk()
        {
            // Arrange
            var dept = CreateDepartment("Ops");
            _uowMock.Setup(u => u.Departments.GetByIdAsync(dept.Id, null))
                .ReturnsAsync(dept);
            _uowMock.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);
            var req = new DepartmentRequestDto { Name = "Ops2", Description = "Operations 2" };

            // Act
            var result = await _service.UpdateAsync(dept.Id, req);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(req.Name, result.Data.Name);
        }

        [Fact]
        public async Task UpdateAsync_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _uowMock.Setup(u => u.Departments.GetByIdAsync(id, null))
                .ReturnsAsync((Department)null);
            var req = new DepartmentRequestDto();

            // Act
            var result = await _service.UpdateAsync(id, req);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.False(result.Data != default);
            Assert.Equal("Department not found", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteAsync_Existing_ReturnsNoContent()
        {
            // Arrange
            var dept = CreateDepartment("R&D");
            _uowMock.Setup(u => u.Departments.GetByIdAsync(dept.Id, null))
                .ReturnsAsync(dept);
            _uowMock.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(dept.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _uowMock.Setup(u => u.Departments.GetByIdAsync(id, null))
                .ReturnsAsync((Department)null);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.False(result.Data);
            Assert.Equal("Department not found", result.ErrorMessage);
        }

        private Department CreateDepartment(string name)
        {
            return new Department(name, $"{name} Dept Description");
        }
    }
}
