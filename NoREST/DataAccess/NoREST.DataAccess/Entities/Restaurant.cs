using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.DataAccess.Entities
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string City { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public int? CreatedById { get; set; }
        [InverseProperty(nameof(User.Restaurants))]
        public virtual User CreatedBy { get; set; }
        [InverseProperty(nameof(UserRestaurantBan.Restaurant))]
        public virtual ICollection<UserRestaurantBan> BannedUsers { get; set; }
        [InverseProperty(nameof(Review.Restaurant))]
        public ICollection<Review> Reviews { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
