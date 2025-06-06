using HRManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(p => p.IsAdmin)
                    // Convert from bit to string
                    .HasConversion(
                        v => v ? "true" : "false",
                        v => v == "true"
                    ).HasMaxLength(5);
            });
        }
    }
}
