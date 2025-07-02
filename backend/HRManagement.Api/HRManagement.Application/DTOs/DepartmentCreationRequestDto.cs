using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class DepartmentCreationRequestDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
