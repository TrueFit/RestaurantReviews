using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoREST.DataAccess.Entities
{
    public class UserRestaurantBan
    {
        [Required]
        [ForeignKey(nameof(Entities.User.Id))]
        public int UserId { get; set; }
        [Required]
        [ForeignKey(nameof(Entities.Restaurant.Id))]
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual User User { get; set; }
        [StringLength(50)]
        public string Reason { get; set; }
        public virtual User CreatedBy { get; set; }
        [ForeignKey(nameof(Entities.User.Id))]
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
