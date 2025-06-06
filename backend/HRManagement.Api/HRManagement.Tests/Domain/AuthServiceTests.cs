using HRManagement.Infrastructure.Context;
using HRManagement.Infrastructure.Services;
using HRManagement.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Tests.Domain
{
    public class AuthServiceTests
    {
        private readonly AppDbContext _dbContext;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            // InMemoryDb context
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);
            var unitOfWork = new UnitOfWork(_dbContext);
            _service = new AuthService(unitOfWork);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
