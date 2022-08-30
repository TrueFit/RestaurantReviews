using NoREST.Models.DomainModels;
using NoREST.Models.Interfaces;
using NoREST.Models.ViewModels.Outgoing;

namespace NoREST.Models.ViewModels.Profile
{
    public class RestaurantProfile : IEntity<int>, IOwnedEntity<UserProfile, int>
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public ICollection<UserRestaurantBanModel> BannedUsers { get; set; }
        public UserProfile CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public bool IsActive { get; set; }
    }
}
