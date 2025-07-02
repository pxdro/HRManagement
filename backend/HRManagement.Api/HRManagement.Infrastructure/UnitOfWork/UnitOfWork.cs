using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Infrastructure.Context;
using HRManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HRManagement.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        public IRepository<Employee> Employees { get; }
        public IRepository<Department> Departments { get; }

        public AppDbContext Context => _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Employees = new Repository<Employee>(_context);
            Departments = new Repository<Department>(_context);
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
