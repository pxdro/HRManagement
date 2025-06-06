using HRManagement.Application.Interfaces;
using HRManagement.Infrastructure.Context;
using HRManagement.Infrastructure.Services;
using HRManagement.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HRManagement.Tests.Domain
{
    public class EmployeeServiceTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly EmployeeService _service;
        public EmployeeServiceTests()
        {
            // InMemoryDb context
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);
            var unitOfWork = new UnitOfWork(_dbContext);
            _service = new EmployeeService(unitOfWork);
        }

        [Fact]
        public async Task GetAllAsync_ThrowsException()
        {
            // Assert
            await Assert.ThrowsAsync<NotImplementedException>(() =>
                _service.GetAllAsync()
            );
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
