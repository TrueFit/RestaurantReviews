using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.DataAccess.Repositories;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels;
using NoREST.Models.ViewModels.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Domain
{
    public interface IBanManager
    {
        Task<ApiResult<bool>> BanUserFromRestaurant(int targetUserId, int restaurantId, string reason, UserProfile currentUser);
    }

    public class BanManager : IBanManager
    {
        private readonly IRestaurantLogic _restaurantLogic;
        private readonly IUserLogic _userLogic;
        private readonly IPermissionLogic _permissionLogic;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogic _auditLogic;

        public BanManager(IRestaurantLogic restaurantLogic, IUserLogic userLogic, IPermissionLogic permissionLogic, IMapper mapper, IUserRepository userRepository, IAuditLogic auditLogic)
        {
            _restaurantLogic = restaurantLogic;
            _userLogic = userLogic;
            _permissionLogic = permissionLogic;
            _mapper = mapper;
            _userRepository = userRepository;
            _auditLogic = auditLogic;
        }

        public async Task<ApiResult<bool>> BanUserFromRestaurant(int targetUserId, int restaurantId, string reason, UserProfile currentUser)
        {
            var restaurant = await _restaurantLogic.GetRestaurant(restaurantId);
            if (restaurant == null)
            {
                return new ApiResult<bool>(false, false, $"Restaurant with Id {restaurantId} does not exist.", System.Net.HttpStatusCode.NotFound);
            }

            var targetUser = await _userLogic.GetUser(targetUserId);
            if (targetUser == null)
            {
                return new ApiResult<bool>(false, false, $"User with Id {targetUserId} does not exist.", System.Net.HttpStatusCode.NotFound);
            }

            if (restaurant.BannedUsers.Any(bu => bu.UserId == targetUser.Id)) return new ApiResult<bool>(true);

            if (_permissionLogic.CanModify(restaurant, currentUser))
            {
                var ban = new UserRestaurantBanModel(targetUser, restaurantId, currentUser, reason);
                _auditLogic.SetAuditValues(ban);
                var success = await _userRepository.BanUserFromRestaurant(_mapper.Map<UserRestaurantBan>(ban));
                var statusCode = success ? HttpStatusCode.OK : HttpStatusCode.Conflict;
                return new ApiResult<bool>(success, success, null, statusCode);
            }
            else
            {
                return new ApiResult<bool>(false, false, $"Current user does not have permission to blocks reviews for restauant {restaurant.Name}.", System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}
