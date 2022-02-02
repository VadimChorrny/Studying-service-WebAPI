using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class VerifiEmailDto
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string Token { get; set; }
    }
}