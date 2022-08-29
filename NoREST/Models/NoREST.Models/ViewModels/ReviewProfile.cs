using NoREST.Models.Interfaces;

namespace NoREST.Models.ViewModels
{
    public class ReviewProfile : IOwnedEntity<UserProfile, int>, IAuditEntity
    { 
        public string Content { get; set; }
        public int RestaurantId { get; set; }
        public int Id { get; set; }
        public UserProfile CreatedBy {get; set;}
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
