using System;
using System.Collections.Generic;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public interface ICityRepository
    {
        CityModel GetByGuid(Guid guid);
        IEnumerable<CityModel> GetAll();
        IEnumerable<CityModel> GetByStateAbbr(string abbr);
    }
}
