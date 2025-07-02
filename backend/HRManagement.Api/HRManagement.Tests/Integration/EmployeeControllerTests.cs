using FluentAssertions;
using HRManagement.Application.DTOs;
using HRManagement.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace HRManagement.Tests.Integration
{
    public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EmployeeControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Testing")).CreateClient();
        }

        private async Task AuthenticateAsync()
        {
            var payload = new LoginDto
            {
                Email = "validemail@example.com",
                Password = "ValidPass123"
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/auth/login", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ResultDto<TokensDto>>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.AuthToken);
        }

        private async Task<Guid> GetDepartmentId()
        {
            var response = await _client.GetAsync("/api/department");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ResultDto<IEnumerable<DepartmentReturnDto>>>();
            return result.Data.First().Id;
        }

        [Fact]
        public async Task GetAll_GetById_Create_Update_Delete_ValidData_ReturnsSuccess()
        {
            // Arrange all
            await AuthenticateAsync();
            var departmentId = await GetDepartmentId();

            /* Get All */
            // Act
            var response = await _client.GetAsync("/api/employee");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<ResultDto<IEnumerable<EmployeeReturnDto>>>();
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.First().Department.Should().NotBeNull();

            /* Create */
            // Arrange
            var createDto = new EmployeeCreationRequestDto 
            { 
                Name = "Test Employee",
                Email = "test@example.com",
                Password = "AnyPassword123",
                Position = "Test Position",
                HireDate = DateTime.UtcNow,
                IsAdmin = false,
                DepartmentId = departmentId,
            };
            var content = JsonContent.Create(createDto);

            // Act
            var createResponse = await _client.PostAsync("/api/employee", content);

            // Assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<EmployeeReturnDto>>();
            created.Should().NotBeNull();

            /* Update */
            // Arrange
            var createdId = created.Data.Id;
            var updateDto = new EmployeeCreationRequestDto
            {
                Name = "Other Test Employee",
                Email = "test@example.com",
                Password = "AnyPassword123",
                Position = "Test Position",
                HireDate = DateTime.UtcNow,
                IsAdmin = false,
                DepartmentId = departmentId,
            };

            // Act
            var updateResponse = await _client.PutAsJsonAsync($"/api/employee/{createdId}", updateDto);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await updateResponse.Content.ReadFromJsonAsync<ResultDto<EmployeeReturnDto>>();
            updated.Data.Name.Should().Be(updateDto.Name);

            /* Get by Id */
            // Act
            var getByIdResponse = await _client.GetAsync($"/api/employee/{createdId}");

            // Assert
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getById = await getByIdResponse.Content.ReadFromJsonAsync<ResultDto<EmployeeReturnDto>>();
            getById.Data.Name.Should().Be(updateDto.Name);
            getById.Data.Department.Should().NotBeNull();

            /* Delete */
            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/employee/{createdId}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetById_Update_Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange all
            await AuthenticateAsync();
            var departmentId = await GetDepartmentId();
            var invalidId = Guid.NewGuid();

            /* Get By Id */
            // Act
            var getByIdResponse = await _client.GetAsync($"/api/employee/{invalidId}");

            // Assert
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            /* Update */
            // Arrange
            var dto = new EmployeeCreationRequestDto
            {
                Name = "Test Employee",
                Email = "test@example.com",
                Password = "AnyPassword123",
                Position = "Test Position",
                HireDate = DateTime.UtcNow,
                IsAdmin = false,
                DepartmentId = departmentId,
            };

            // Act
            var updateResponse = await _client.PutAsJsonAsync($"/api/employee/{invalidId}", dto);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            /* Delete */
            // Act
            var deleteResponse = await _client.GetAsync($"/api/employee/{invalidId}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
