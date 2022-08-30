using System.ComponentModel.DataAnnotations;

namespace NoREST.Models.ViewModels.Creation
{
    public class RestaurantCreation
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string City { get; set; }
    }
}
