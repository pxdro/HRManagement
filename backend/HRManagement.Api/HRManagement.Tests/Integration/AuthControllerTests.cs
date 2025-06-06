using FluentAssertions;
using HRManagement.Application.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace HRManagement.Tests.Integration
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            // Ensure Testing environment
            _client = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Testing")).CreateClient();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkAndToken()
        {
            // Arrange
            var payload = new LoginDto
            {
                Email = "validemail@example.com",
                Password = "ValidPass123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<ResultDto<TokensDto>>();
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.AuthToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var payload = new LoginDto
            {
                Email = "validemail@example.com",
                Password = "WrongPass123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var payload = new LoginDto
            {
                Email = "usernotexist@example.com",
                Password = "AnyPass123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
