using System.ComponentModel.DataAnnotations;

namespace NoREST.Models.ViewModels.Creation
{
    public class UserCreation
    {
        [Required]
        [StringLength(25)]
        public string Username { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
