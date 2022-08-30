using System.ComponentModel.DataAnnotations;

namespace NoREST.Models.ViewModels.Creation
{
    public class ReviewCreation
    {
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        [StringLength(500)]
        public string Content { get; set; }
    }
}
