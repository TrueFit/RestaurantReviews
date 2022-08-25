using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.DataAccess.Repositories;
using NoREST.Models;

namespace NoREST.Domain
{
    public interface IUserLogic
    {
        Task<UserProfile> Create(UserCreation user);
        Task<UserProfile> GetUser(int userId);
        Task<UserProfile> GetUserProfileFromIdentityProviderId(string identityProviderId);
    }

    public class UserLogic : IUserLogic
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityProviderService _identityProviderService;

        public UserLogic(IMapper mapper, IUserRepository userRepository, IIdentityProviderService identityProviderService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _identityProviderService = identityProviderService;
        }

        public async Task<UserProfile> GetUserProfileFromIdentityProviderId(string identityProviderId)
        {
            var user = await _userRepository.GetUserByIdentityProviderId(identityProviderId);
            return _mapper.Map<UserProfile>(user);
        }

        public async Task<UserProfile> GetUser(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            return _mapper.Map<UserProfile>(user);
        }

        public async Task<UserProfile> Create(UserCreation userCreation)
        {
            await _identityProviderService.CreateUser(userCreation);
            var user = await _userRepository.CreateUser(_mapper.Map<User>(userCreation));
            return _mapper.Map<UserProfile>(user);
        }

    }
}