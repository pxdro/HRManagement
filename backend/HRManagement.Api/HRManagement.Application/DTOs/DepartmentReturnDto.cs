namespace HRManagement.Application.DTOs
{
    public class DepartmentReturnDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string RowVersion { get; set; }
    }
}
