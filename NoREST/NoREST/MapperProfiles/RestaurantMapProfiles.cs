using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models.ViewModels.Creation;
using NoREST.Models.ViewModels.Profile;

namespace NoREST.Api.MapperProfiles
{
    public class RestaurantMapProfile : Profile
    {
        public RestaurantMapProfile()
        {
            CreateMap<RestaurantProfile, Restaurant>().ReverseMap();
            CreateMap<RestaurantCreation, RestaurantProfile>();
        }
    }
}
