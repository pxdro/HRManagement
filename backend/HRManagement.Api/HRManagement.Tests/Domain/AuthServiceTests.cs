﻿using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace HRManagement.Tests.Domain
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly Mock<IConfiguration> _configMock;

        public AuthServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");
            _configMock.Setup(c => c["JWT_SECRET"]).Returns("test_jwt_secret_key_1234567890_test_jwt_secret_key_1234567890_test_jwt_secret_key_1234567890");
        }

        [Fact]
        public void Constructor_MissingSecret_ThrowsInvalidOperationException()
        {
            // Arrange
            _configMock.Setup(c => c["JWT_SECRET"]).Returns((string)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                new AuthService(_configMock.Object, _uowMock.Object, _loggerMock.Object)
            );
        }

        [Fact]
        public void Constructor_MissingJWTIssuer_ThrowsInvalidOperationException()
        {
            // Arrange
            _configMock.Setup(c => c["JwtSettings:Issuer"]).Returns((string)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                new AuthService(_configMock.Object, _uowMock.Object, _loggerMock.Object)
            );
        }

        [Fact]
        public void Constructor_MissingJWTAudience_ThrowsInvalidOperationException()
        {
            // Arrange
            _configMock.Setup(c => c["JwtSettings:Audience"]).Returns((string)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                new AuthService(_configMock.Object, _uowMock.Object, _loggerMock.Object)
            );
        }

        [Fact]
        public async Task LoginAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _uowMock.Setup(u => u.Employees.GetAllAsync(null))
                .ReturnsAsync(new List<Employee>());
            var auth = new AuthService(_configMock.Object, _uowMock.Object, _loggerMock.Object);
            var dto = new LoginDto { Email = "noone@test.com", Password = "pass" };

            // Act
            var result = await auth.LoginAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("User not found", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var employee = new Employee("User", "user@test.com", BCrypt.Net.BCrypt.HashPassword("correct"), "Role", DateTime.UtcNow.AddMonths(-1), false, Guid.NewGuid());
            _uowMock.Setup(u => u.Employees.GetAllAsync(null))
                .ReturnsAsync(new List<Employee> { employee });
            var auth = new AuthService(_configMock.Object, _uowMock.Object, _loggerMock.Object);
            var dto = new LoginDto { Email = "user@test.com", Password = "wrong" };

            // Act
            var result = await auth.LoginAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Invalid credentials", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var plain = "secretPW";
            var employee = new Employee("User", "user@test.com", BCrypt.Net.BCrypt.HashPassword(plain), "Role", DateTime.UtcNow.AddMonths(-1), true, Guid.NewGuid());
            _uowMock.Setup(u => u.Employees.GetAllAsync(null))
                .ReturnsAsync(new List<Employee> { employee });
            var auth = new AuthService(_configMock.Object, _uowMock.Object, _loggerMock.Object);
            var dto = new LoginDto { Email = "user@test.com", Password = plain };

            // Act
            var result = await auth.LoginAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Data.AuthToken);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Email && c.Value == employee.Email);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == employee.Id.ToString());
        }

        [Fact]
        public async Task LoginAsync_OnException_ReturnsFailureAndLogsError()
        {
            // Arrange
            _uowMock.Setup(u => u.Employees.GetAllAsync(null))
                .ThrowsAsync(new Exception("DB error"));
            var auth = new AuthService(_configMock.Object, _uowMock.Object, _loggerMock.Object);
            var dto = new LoginDto { Email = "any@test.com", Password = "pw" };

            // Act
            var result = await auth.LoginAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
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
    }
}
