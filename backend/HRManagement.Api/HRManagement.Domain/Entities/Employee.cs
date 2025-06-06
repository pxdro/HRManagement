using System.ComponentModel.DataAnnotations;

namespace HRManagement.Domain.Entities
{
    public class Employee : Entity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; private set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; private set; }

        [Required]
        public string HashedPassword { get; private set; }

        [Required]
        [MaxLength(150)]
        public string Position { get; private set; }

        [Required]
        public DateTime HireDate { get; private set; }

        [Required]
        public bool IsAdmin { get; private set; }

        [Required]
        public Guid DepartmentId { get; private set; }

        public Department Department { get; internal set; } // Internal for tests

        /* For future: RefreshToken
        public string? RefreshToken { get; private set; }

        public DateTime? RefreshTokenExpiry { get; private set; }
        */

        public Employee(
            string name,
            string email,
            string hashedPassword,
            string position,
            DateTime hireDate,
            bool isAdmin,
            Guid departmentId)
        {
            Name = name;
            Email = email;
            HashedPassword = hashedPassword;
            Position = position;
            HireDate = hireDate;
            IsAdmin = isAdmin;
            DepartmentId = departmentId;
        }

        public void Update(
            string name,
            string email,
            string hashedPassword,
            string position,
            DateTime hireDate,
            bool isAdmin,
            Guid departmentId)
        {
            Name = name;
            Email = email;
            HashedPassword = hashedPassword;
            Position = position;
            HireDate = hireDate;
            IsAdmin = isAdmin;
            DepartmentId = departmentId;
        }
    }

}
