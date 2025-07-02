using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class TokensDto
    {
        [Required]
        public string AuthToken { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string RefreshToken { get; set; } = null!;
    }
}
