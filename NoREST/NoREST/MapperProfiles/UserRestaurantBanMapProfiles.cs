using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models.DomainModels;

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
