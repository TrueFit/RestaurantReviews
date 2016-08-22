using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReviews.Data {
    
    /// <summary>
    /// User Model class
    /// </summary>
    [Table("User")]
    public partial class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        public bool Locked { get; set; }
    }
}