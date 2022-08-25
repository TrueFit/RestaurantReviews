using System.ComponentModel.DataAnnotations;

namespace NoREST.DataAccess.Entities
{
    public class User : IEntity<int>
    {
        [Key]        
        public int Id { get; set; }
        public string IdentityProviderId { get; set; }
        public string UserName { get; set; }
        public bool IsSystemUser { get; set; }
        public bool IsAdmin { get; set; }
    }
}
