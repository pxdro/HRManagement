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
                    .HasConversion(
                        v => v ? "true" : "false",
                        v => v == "true"
                    ).HasMaxLength(5);

                entity.HasIndex(d => d.Name)
                      .HasDatabaseName("IX_Employees_Name");
            });
        }
    }
}
