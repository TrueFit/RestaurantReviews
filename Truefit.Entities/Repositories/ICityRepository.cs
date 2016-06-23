using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public interface ICityRepository
    {
        Task<CityModel> GetByGuid(Guid guid);
        Task<IEnumerable<CityModel>> GetAll();
        Task<IEnumerable<CityModel>> GetByStateAbbr(string abbr);
    }
}
