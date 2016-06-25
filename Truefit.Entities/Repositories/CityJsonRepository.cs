using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biggy.Core;
using Biggy.Data.Json;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public class CityJsonRepository : ICityRepository
    {
        private readonly BiggyList<CityModel> _cities; 

        public CityJsonRepository(JsonDbCore dbCore)
        {
            var store = new JsonStore<CityModel>(dbCore);
            this._cities = new BiggyList<CityModel>(store);
        }

        public async Task<CityModel> GetByGuid(Guid guid)
        {
            return this._cities.FirstOrDefault(x => x.Guid == guid);
        }

        public async Task<IEnumerable<CityModel>> GetAll()
        {
            return this._cities;
        }
    }
}
