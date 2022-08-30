using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.DataAccess.Repositories;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels;
using NoREST.Models.ViewModels.Creation;
using NoREST.Models.ViewModels.Outgoing;
using NoREST.Models.ViewModels.Profile;

namespace NoREST.Domain
{
    public interface IUserLogic
    {
        Task<ApiResult<bool>> BanUserFromRestaurant(UserProfile targetUser, RestaurantProfile restaurantProfile, string reason, UserProfile currentUser);
        Task<ApiResult<UserProfile>> CreateUser(UserCreation user);
        Task<UserProfile> GetUser(int userId);
        Task<UserProfile> GetUserProfileFromIdentityProviderId(string identityProviderId);
    }

    public class UserLogic : IUserLogic
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityProviderService _identityProviderService;
        private readonly IPermissionLogic _permissionLogic;
        private readonly IAuditLogic _auditLogic;

        public UserLogic(IMapper mapper, IUserRepository userRepository, IIdentityProviderService identityProviderService, IPermissionLogic permissionLogic, IAuditLogic auditLogic)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _identityProviderService = identityProviderService;
            _permissionLogic = permissionLogic;
            _auditLogic = auditLogic;
        }

        public async Task<UserProfile> GetUserProfileFromIdentityProviderId(string identityProviderId)
        {
            var user = await _userRepository.GetUserByIdentityProviderId(identityProviderId);
            return user == null ? null : _mapper.Map<UserProfile>(user);
        }

        public async Task<UserProfile> GetUser(int userId)
        {
            return await _userRepository.GetUser(userId);
        }

        public async Task<ApiResult<UserProfile>> CreateUser(UserCreation userCreation)
        {
            var response = await _identityProviderService.CreateUser(userCreation);
            if (response.IsSuccess)
            {
                var entity = _mapper.Map<User>(userCreation);
                entity.IdentityProviderId = response.IdentityProviderId;
                try
                {
                    var user = await _userRepository.CreateUser(entity);
                    if (user != null)
                    {
                        return new ApiResult<UserProfile>(_mapper.Map<UserProfile>(user));
                    }
                }
                catch { /* being double-extra careful */ }

                // If user is null, or any exception was thrown, we come to this block.
                // we remove the user from the identity pool to prevent our system from getting out of sync with the idp.
                await _identityProviderService.RemoveUser(userCreation); //if this fails, I guess you're sol... so we return 409 either way
                return new ApiResult<UserProfile>(null, false, response.Error, System.Net.HttpStatusCode.Conflict);
            }

            return new ApiResult<UserProfile>(null, false, response.Error, System.Net.HttpStatusCode.InternalServerError);
        }

        public async Task<ApiResult<bool>> BanUserFromRestaurant(UserProfile targetUser, RestaurantProfile restaurantProfile, string reason, UserProfile currentUser)
        {
            if (restaurantProfile.BannedUsers.Any(bu => bu.UserId == targetUser.Id)) return new ApiResult<bool>(true);

            if (_permissionLogic.CanModify(restaurantProfile, currentUser))
            {
                var ban = new UserRestaurantBanModel(targetUser, restaurantProfile.Id, currentUser, reason);
                _auditLogic.SetAuditValues(ban);
                var success = await _userRepository.BanUserFromRestaurant(_mapper.Map<UserRestaurantBan>(ban));
                return new ApiResult<bool>(success, success);
            }
            else
            {
                return new ApiResult<bool>(false, false, $"Current user does not have permission to blocks reviews for restauant {restaurantProfile.Name}.", System.Net.HttpStatusCode.Unauthorized);
            }
        }

    }
}