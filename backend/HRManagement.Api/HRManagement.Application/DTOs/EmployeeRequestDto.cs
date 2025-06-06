using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class EmployeeRequestDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [MaxLength(150)]
        public string Position { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }
    }
}
