﻿namespace HRManagement.Application.DTOs
{
    public class EmployeeReturnDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsAdmin { get; set; }

        public Guid DepartmentId { get; set; }

        public string RowVersion { get; set; }

        public DepartmentReturnDto Department { get; set; }
    }
}
