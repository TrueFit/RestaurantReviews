using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoREST.DataAccess.Entities
{
    public class User
    {
        [Key]        
        public int Id { get; set; }
        [StringLength(100)]
        public string IdentityProviderId { get; set; }
        [StringLength(25)]
        public string UserName { get; set; }
        public bool IsSystemUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Restaurant> Restaurants { get; set; }

        public virtual ICollection<UserRestaurantBan> BannedRestaurants { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
