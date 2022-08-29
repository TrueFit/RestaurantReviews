using NoREST.Models.Interfaces;

namespace NoREST.Models.ViewModels
{
    public class RestaurantProfile : IEntity<int>, IOwnedEntity<UserProfile, int>
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public ICollection<UserRestaurantBanModel> BannedUsers { get; set; }
        public UserProfile CreatedBy { get; set; }
        public int CreatedById { get; set; }
    }
}
