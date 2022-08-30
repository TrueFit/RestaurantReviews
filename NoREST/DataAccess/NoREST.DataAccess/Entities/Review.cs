using NoREST.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoREST.DataAccess.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string Content { get; set; }
        [ForeignKey(nameof(Entities.Restaurant.Id))]
        public int? RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        [ForeignKey(nameof(User.Id))]
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public bool IsActive { get; set; } = true; //Will this appear in migrations?
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
