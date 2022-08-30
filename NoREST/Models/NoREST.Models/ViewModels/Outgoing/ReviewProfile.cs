using NoREST.Models.Interfaces;
using NoREST.Models.ViewModels.Outgoing;

namespace NoREST.Models.ViewModels.Profile
{
    public class ReviewProfile : IOwnedEntity<UserProfile, int>, IAuditEntity
    {
        public string Content { get; set; }
        public int RestaurantId { get; set; }
        public int Id { get; set; }
        public UserProfile CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
