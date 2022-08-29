using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models.ViewModels;

namespace NoREST.Api.MapperProfiles
{
    public class UserRestaurantBanMapProfiles : Profile
    {
        public UserRestaurantBanMapProfiles()
        {
            CreateMap<UserRestaurantBan, UserRestaurantBanModel>().ReverseMap();
        }
    }
}
