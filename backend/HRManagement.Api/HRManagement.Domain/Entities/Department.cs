using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HRManagement.Domain.Entities
{
    public class Department : Entity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; private set; }

        [MaxLength(250)]
        public string? Description { get; private set; }

        [JsonIgnore]
        public ICollection<Employee> Employees { get; private set; } = new List<Employee>();

        public Department(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }

        public void Update(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }
}