using System.ComponentModel.DataAnnotations;

namespace HRManagement.Application.DTOs
{
    public class TokensDto
    {
        [Required]
        public string? AuthToken { get; set; }

        /* For future: RefreshToken
        [Required]
        public string? RefreshToken { get; set; }
        */
    }
}
