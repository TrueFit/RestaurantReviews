using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoREST.DataAccess.Entities;
using NoREST.Models.ViewModels.Outgoing;

namespace NoREST.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<UserProfile> CreateUser(User user);
        Task<UserProfile> GetUserByIdentityProviderId(string identityProviderId);
        Task<UserProfile> GetUser(int userId);
        Task<bool> BanUserFromRestaurant(UserRestaurantBan userRestaurantBan);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<NoRESTContext> _dbFactory;
        private readonly ILogger<IUserRepository> _logger;
        private readonly IMapper _mapper;

        public UserRepository(IDbContextFactory<NoRESTContext> dbFactory, ILogger<IUserRepository> logger, IMapper mapper)
        {
            _dbFactory = dbFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> BanUserFromRestaurant(UserRestaurantBan userRestaurantBan)
        {
            try
            {
                using (var context = _dbFactory.CreateDbContext())
                {
                    context.UserRestaurantBans.Add(userRestaurantBan);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected Error attempting to ban user {userRestaurantBan.UserId} from Restaurant {userRestaurantBan.RestaurantId}.");
                return false;
            }
        }

        public async Task<UserProfile> CreateUser(User user)
        {
            try
            {
                using (var context = _dbFactory.CreateDbContext())
                {
                    context.Add(user);
                    await context.SaveChangesAsync();
                    return _mapper.Map<UserProfile>(user);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected Error attempting to create user {user.UserName}");
                return null;
            }
            
        }

        public async Task<UserProfile> GetUser(int userId)
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                return user == null ? null : _mapper.Map<UserProfile>(user);
            }
            
        }

        public async Task<UserProfile> GetUserByIdentityProviderId(string identityProviderId)
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.IdentityProviderId == identityProviderId);
                return user == null ? null : _mapper.Map<UserProfile>(user);
            }
                
        }
    }
}
