﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data
{
    public interface IEntityService
    {
        /// <summary>
        /// Get a City by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<CityModel> GetCity(Guid guid);

        /// <summary>
        /// Returns all known Cities
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CityModel>> GetAllCities();

        /// <summary>
        /// Gets an Entity by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<EntityModel> GetEntity(Guid guid);

        /// <summary>
        /// Gets all Entities for a City that have a specific Type
        /// </summary>
        /// <param name="city"></param>
        /// <param name="type">Restuarant, Yoga Studio, etc. (free text)</param>
        /// <returns></returns>
        Task<IEnumerable<EntityModel>> GetEntities(CityModel city, string type);

    }
}
