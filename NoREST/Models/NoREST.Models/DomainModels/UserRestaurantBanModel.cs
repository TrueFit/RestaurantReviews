using NoREST.Models.Interfaces;
using NoREST.Models.ViewModels.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Models.DomainModels
{
    public class UserRestaurantBanModel : IAuditEntity
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public string Reason { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }

        public UserRestaurantBanModel() { }

        public UserRestaurantBanModel(UserProfile targetUser, int restaurantId, UserProfile requestingUser, string reason)
        {
            UserId = targetUser.Id;
            RestaurantId = restaurantId;
            Reason = reason;
            CreatedById = requestingUser.Id;
        }
    }
}
