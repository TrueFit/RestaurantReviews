using AutoMapper;
using RestaurantReview.Models;
using RestaurantReview.Models.ResponseModels;
using RestaurantReview.Models.UserModels;
using RestaurantReview.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            ConfigureUserMappings();
        }

        private static void ConfigureUserMappings()
        {
            Mapper.CreateMap<CreateUserModel, SafeUserModel>();
            Mapper.CreateMap<LoginUserModel, SafeUserModel>();
            Mapper.CreateMap<Session, UserLoginResponseModel>()
                .ForMember(dest => dest.access_token, opts => opts.MapFrom(src => src.AuthToken))
                .ForMember(dest => dest.expires_in, opts => opts.MapFrom(src => (src.EndTime - DateTime.Now).TotalSeconds))
                .ForMember(dest => dest.token_type, opts => opts.MapFrom(src => SessionUtilities.TokenType));
        }
    }
}