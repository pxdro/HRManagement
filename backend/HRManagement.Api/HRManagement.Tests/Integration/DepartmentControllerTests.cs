using FluentAssertions;
using HRManagement.Application.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace HRManagement.Tests.Integration
{
    public class DepartmentControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DepartmentControllerTests(WebApplicationFactory<Program> factory)
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

        [Fact]
        public async Task GetAll_GetById_Create_Update_Delete_ValidData_ReturnsSuccess()
        {
            // Arrange all
            await AuthenticateAsync();

            /* Get All */
            // Act
            var response = await _client.GetAsync("/api/department");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<ResultDto<IEnumerable<DepartmentReturnDto>>>();
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();

            /* Create */
            // Arrange
            var createDto = new DepartmentCreationRequestDto { Name = "Test Department" };
            var content = JsonContent.Create(createDto);

            // Act
            var createResponse = await _client.PostAsync("/api/department", content);

            // Assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await createResponse.Content.ReadFromJsonAsync<ResultDto<DepartmentReturnDto>>();
            created.Should().NotBeNull();

            /* Update */
            // Arrange
            var createdId = created.Data.Id;
            var updateDto = new DepartmentCreationRequestDto { Name = "Updated Department" };

            // Act
            var updateResponse = await _client.PutAsJsonAsync($"/api/department/{createdId}", updateDto);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await updateResponse.Content.ReadFromJsonAsync<ResultDto<DepartmentReturnDto>>();
            updated.Data.Name.Should().Be(updateDto.Name);

            /* Get by Id */
            // Act
            var getByIdResponse = await _client.GetAsync($"/api/department/{createdId}");

            // Assert
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getById = await getByIdResponse.Content.ReadFromJsonAsync<ResultDto<DepartmentReturnDto>>();
            getById.Data.Name.Should().Be(updateDto.Name);

            /* Delete */
            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/department/{createdId}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetById_Update_Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange all
            await AuthenticateAsync();
            var invalidId = Guid.NewGuid();

            /* Get By Id */
            // Act
            var getByIdResponse = await _client.GetAsync($"/api/department/{invalidId}");

            // Assert
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            /* Update */
            // Arrange
            var updateDto = new DepartmentCreationRequestDto { Name = "Updated Department" };

            // Act
            var updateResponse = await _client.PutAsJsonAsync($"/api/department/{invalidId}", updateDto);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            /* Delete */
            // Act
            var deleteResponse = await _client.GetAsync($"/api/department/{invalidId}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
