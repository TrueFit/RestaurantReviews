using AutoMapper;
using RestaurantReview.Models.UserModels;
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
        }
    }
}