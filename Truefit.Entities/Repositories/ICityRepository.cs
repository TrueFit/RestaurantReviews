using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public interface ICityRepository
    {
        /// <summary>
        /// Gets a City by its Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<CityModel> GetByGuid(Guid guid);

        /// <summary>
        /// Gets all Cities in Repository
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CityModel>> GetAll();

        /// <summary>
        /// Gets a City by a US State abbreviation
        /// </summary>
        /// <param name="abbr"></param>
        /// <returns></returns>
        Task<IEnumerable<CityModel>> GetByStateAbbr(string abbr);
    }
}
