using AutoMapper;
using RestaurantReview.Models;
using RestaurantReview.Models.CommentModels;
using RestaurantReview.Models.CustomRestRevModels;
using RestaurantReview.Models.ResponseModels;
using RestaurantReview.Models.ReviewModels;
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
        private static RestRevEntities db = new RestRevEntities();

        public static void Configure()
        {
            ConfigureUserMappings();
            ConfigureRestaurantMappings();
            ConfigureReviewMappings();
            ConfigureCommentMappings();
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

        private static void ConfigureRestaurantMappings()
        {
            Mapper.CreateMap<Restaurant, DisplayRestaurantModel>()
                .ForMember(dest => dest.ReviewIds, opts => opts.MapFrom(src => src.Reviews.Select(r => r.Id).ToList()))
                .ForMember(dest => dest.Tags, opts => opts.MapFrom(src => src.Tags.Select(t => t.TagName).ToList()))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opts => opts.MapFrom(src => src.State))
                .ForMember(dest => dest.StreetAddress1, opts => opts.MapFrom(src => src.StreetAddress1))
                .ForMember(dest => dest.StreetAddress2, opts => opts.MapFrom(src => src.StreetAddress2))
                .ForMember(dest => dest.PhoneNum, opts => opts.MapFrom(src => src.PhoneNum))
                .ForMember(dest => dest.OwnerUserName, opts => opts.MapFrom(src => src.OwnerUserName));
        }

        private static void ConfigureReviewMappings()
        {
            Mapper.CreateMap<CreateReviewModel, Review>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => 0))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.RestaurantId, opts => opts.MapFrom(src => src.RestaurantId))
                .ForMember(dest => dest.Restaurant, opts => opts.MapFrom(src => db.Restaurants.Find(src.RestaurantId)))
                .ForMember(dest => dest.Rating, opts => opts.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content))
                .ForMember(dest => dest.Timestamp, opts => opts.MapFrom(src => DateTime.Now));

            Mapper.CreateMap<Review, DisplayReviewModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content))
                .ForMember(dest => dest.RestaurantId, opts => opts.MapFrom(src => src.RestaurantId))
                .ForMember(dest => dest.Rating, opts => opts.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Timestamp, opts => opts.MapFrom(src => src.Timestamp.ToString()))
                .ForMember(dest => dest.CommentIds, opts => opts.MapFrom(src => src.Comments.Select(c => c.Id).ToList()));
        }

        private static void ConfigureCommentMappings()
        {
            Mapper.CreateMap<Comment, DisplayCommentModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReviewId, opts => opts.MapFrom(src => src.ReviewId))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content))
                .ForMember(dest => dest.Timestamp, opts => opts.MapFrom(src => src.Timestamp.ToString()));

            Mapper.CreateMap<CreateCommentModel, Comment>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => 0))
                .ForMember(dest => dest.ReviewId, opts => opts.MapFrom(src => src.ReviewId))
                .ForMember(dest => dest.Review, opts => opts.MapFrom(src => db.Reviews.Find(src.ReviewId)))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content))
                .ForMember(dest => dest.Timestamp, opts => opts.MapFrom(src => DateTime.Now));
        }
    }
}