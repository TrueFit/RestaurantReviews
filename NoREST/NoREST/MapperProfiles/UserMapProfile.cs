using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models;

namespace NoREST.Api.MapperProfiles
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserCreation, User>();
            CreateMap<UserProfile, User>().ReverseMap();
        }
    }
}
