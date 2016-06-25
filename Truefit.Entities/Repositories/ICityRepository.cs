using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truefit.Entities.Models;

namespace Truefit.Entities.Repositories
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
    }
}
