using HRManagement.Infrastructure.Context;
using HRManagement.Infrastructure.Services;
using HRManagement.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Tests.Domain
{
    public class DepartmentServiceTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            // InMemoryDb context
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);
            var unitOfWork = new UnitOfWork(_dbContext);
            _service = new DepartmentService(unitOfWork);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
