using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RstrntAPI.DTO;
using RstrntAPI.DataAccess.Models;

namespace RstrntAPI.Repository.Transforms
{
    internal static class RepositoryTransforms
    {
        public static CityDTO ToDTO(this CityEntity item)
        {
            return new CityDTO
            {
                Id = item.id,
                Name = item.name,
                KeyedName = item.keyed_name
            };
        }
        public static CityEntity ToEntity(this CityDTO item)
        {
            return new CityEntity
            {
                id = item.Id,
                name = item.Name,
                keyed_name = item.KeyedName
            };
        }

        public static LocationDTO ToDTO(this LocationsEntity item)
        {
            return new LocationDTO
            {
                Id = item.id,
                CityId = item.city_id,
                RestaurantId = item.restaurant_id,
                StreetAddress = item.street_address
            };
        }
        public static LocationsEntity ToEntity(this LocationDTO item)
        {
            return new LocationsEntity
            {
                id = item.Id,
                city_id = item.CityId,
                restaurant_id = item.RestaurantId,
                street_address = item.StreetAddress
            };
        }

        public static RestaurantDTO ToDTO(this RestaurantsEntity item)
        {
            return new RestaurantDTO
            {
                Id = item.id,
                Name = item.name,
                KeyedName = item.keyed_name
            };
        }
        public static RestaurantsEntity ToEntity(this RestaurantDTO item)
        {
            return new RestaurantsEntity
            {
                id = item.Id,
                name = item.Name,
                keyed_name = item.KeyedName
            };
        }

        public static ReviewDTO ToDTO(this ReviewsEntity item)
        {
            return new ReviewDTO
            {
                Id = item.id,
                LocationId = item.location_id,
                UserId = item.user_id,
                Body = item.body,
                Subject = item.subject
            };
        }
        public static ReviewsEntity ToEntity(this ReviewDTO item)
        {
            return new ReviewsEntity
            {
                id = item.Id,
                location_id = item.LocationId,
                user_id = item.UserId,
                body = item.Body,
                subject = item.Subject
            };
        }

        public static UserDTO ToDTO(this UsersEntity item)
        {
            return new UserDTO
            {
                Id = item.id,
                AccountName = item.acct_name,
                FullName = item.full_name,
                Hometown = item.hometown
            };
        }
        public static UsersEntity ToEntity(this UserDTO item)
        {
            return new UsersEntity
            {
                id = item.Id,
                acct_name = item.AccountName,
                full_name = item.FullName,
                hometown = item.Hometown
            };
        }
    }
}
