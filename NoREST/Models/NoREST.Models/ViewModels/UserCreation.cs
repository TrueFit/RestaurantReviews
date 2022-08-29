using System.ComponentModel.DataAnnotations;

namespace NoREST.Models
{
    public class UserCreation
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
