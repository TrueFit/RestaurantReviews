using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models.ViewModels.Creation;
using NoREST.Models.ViewModels.Outgoing;

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
