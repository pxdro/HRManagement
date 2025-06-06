namespace HRManagement.Application.DTOs
{
    public class EmployeeReturnDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public DateOnly HireDate { get; set; }

        public bool IsAdmin { get; set; }

        public Guid DepartmentId { get; set; }

        public DepartmentDto Department { get; set; }
    }
}
