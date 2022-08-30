using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models.ViewModels.Creation;
using NoREST.Models.ViewModels.Profile;

namespace NoREST.Api.MapperProfiles
{
    public class ReviewMapProfile : Profile
    {
        public ReviewMapProfile()
        {
            CreateMap<ReviewProfile, Review>().ReverseMap();
            CreateMap<ReviewCreation, ReviewProfile>();
        }
    }
}
