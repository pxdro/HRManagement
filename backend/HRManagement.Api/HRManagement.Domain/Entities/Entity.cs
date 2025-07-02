using System.ComponentModel.DataAnnotations;

namespace HRManagement.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}