using NoREST.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Models.ViewModels
{
    public class UserRestaurantBanModel : IAuditEntity
    {       
        public int UserId { get; set; }
        public UserProfile User { get; set; }
        public int RestaurantId { get; set; }
        public string Reason { get; set; }
        public UserProfile CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }

        public UserRestaurantBanModel() { }

        public UserRestaurantBanModel(UserProfile targetUser, int restaurantId, UserProfile requestingUser, string reason)
        {
            UserId = targetUser.Id;
            User = targetUser;
            RestaurantId = restaurantId;
            Reason = reason;
            CreatedBy = requestingUser;
            CreatedById = requestingUser.Id;
        }
    }
}
