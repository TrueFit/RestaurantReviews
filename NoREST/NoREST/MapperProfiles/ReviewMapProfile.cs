using AutoMapper;
using NoREST.DataAccess.Entities;
using NoREST.Models;
using NoREST.Models.ViewModels;

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
