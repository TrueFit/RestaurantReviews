using NoREST.Models.Interfaces;

namespace NoREST.Models.ViewModels.Outgoing
{
    public class UserProfile : IEntity<int>, IPermissioned
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsAdmin { get; set; }
    }
}
