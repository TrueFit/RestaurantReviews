using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public interface IEntityRepository
    {
        /// <summary>
        /// Gets an Entity by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<EntityModel> GetByGuid(Guid guid);

        /// <summary>
        /// Gets a City by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<IEnumerable<EntityModel>> GetByCity(Guid guid);

        /// <summary>
        /// Inserts a City into the Repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Insert(EntityModel entity);
    }
}
