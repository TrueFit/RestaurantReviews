using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models;
using NoREST.Models.ViewModels;

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
