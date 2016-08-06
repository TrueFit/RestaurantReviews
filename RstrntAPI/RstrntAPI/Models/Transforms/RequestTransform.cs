using RstrntAPI.DTO;
using RstrntAPI.Models.Request;
using RstrntAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.Models.Transforms
{
    internal static class RequestTransform
    {
        public static CityRequest ToRequest(this CityDTO item)
        {
            return new CityRequest
            {
                Id = item?.Id,
                Name = item?.Name,
                KeyedName = item?.KeyedName
            };
        }
        public static CityDTO ToDTO(this CityRequest item)
        {
            return new CityDTO
            {
                Id = item?.Id,
                Name = item?.Name,
                KeyedName = item?.KeyedName
            };
        }


        public static LocationRequest ToRequest(this LocationDTO item)
        {
            return new LocationRequest
            {
                Id = item?.Id,
                CityId = item?.CityId,
                RestaurantId = item?.RestaurantId,
                StreetAddress = item?.StreetAddress
            };
        }
        public static LocationDTO ToDTO(this LocationRequest item)
        {
            return new LocationDTO
            {
                Id = item?.Id,
                CityId = item?.CityId ?? -1,
                RestaurantId = item?.RestaurantId ?? -1,
                StreetAddress = item?.StreetAddress
            };
        }

        public static RestaurantRequest ToRequest(this RestaurantDTO item)
        {
            return new RestaurantRequest
            {
                Id = item?.Id,
                Name = item?.Name,
                KeyedName = item?.KeyedName
            };
        }
        public static RestaurantDTO ToDTO(this RestaurantRequest item)
        {
            return new RestaurantDTO
            {
                Id = item?.Id,
                Name = item?.Name,
                KeyedName = item?.KeyedName
            };
        }

        public static ReviewRequest ToRequest(this ReviewDTO item)
        {
            return new ReviewRequest
            {
                Id = item?.Id,
                LocationId = item?.LocationId,
                UserId = item?.UserId,
                Subject = item?.Subject,
                Body = item?.Body
            };
        }
        public static ReviewDTO ToDTO(this ReviewRequest item)
        {
            return new ReviewDTO
            {
                Id = item?.Id,
                LocationId = item?.LocationId ?? -1,
                UserId = item?.UserId ?? -1,
                Subject = item?.Subject,
                Body = item?.Body
            };
        }

        public static UserRequest ToRequest(this UserDTO item)
        {
            return new UserRequest
            {
                Id = item?.Id,
                AccountName = item?.AccountName,
                FullName = item?.FullName,
                Hometown = item?.Hometown
            };
        }
        public static UserDTO ToDTO(this UserRequest item)
        {
            return new UserDTO
            {
                Id = item?.Id,
                AccountName = item?.AccountName,
                FullName = item?.FullName,
                Hometown = item?.Hometown
            };
        }

        public static LocationDetailResponse ToResponse(this LocationDetailDTO item)
        {
            return new LocationDetailResponse
            {
                RestaurantId = item?.RestaurantId,
                LocationId = item?.LocationId,
                CityId = item?.CityId,
                RestaurantName = item?.RestaurantName,
                StreetAddress = item?.StreetAddress,
                City = item?.City,
                Reviews = item?.Reviews.Select(x => x.ToRequest()).ToList()
            };
        }
        public static LocationDetailDTO ToDTO(this LocationDetailResponse item)
        {
            return new LocationDetailDTO
            {
                RestaurantId = item?.RestaurantId,
                LocationId = item?.LocationId,
                CityId = item?.CityId,
                RestaurantName = item?.RestaurantName,
                StreetAddress = item?.StreetAddress,
                City = item?.City,
                Reviews = item?.Reviews.Select(x => x.ToDTO()).ToList()
            };
        }
    }
}
