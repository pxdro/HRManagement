using HRManagement.Domain.Entities;

namespace HRManagement.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Employee> Employees { get; }
        IRepository<Department> Departments { get; }
        Task<int> SaveChangesAsync();
    }
}
