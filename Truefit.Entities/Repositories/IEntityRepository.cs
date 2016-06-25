using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truefit.Entities.Models;

namespace Truefit.Entities.Repositories
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
        /// <param name="cityId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<EntityModel>> GetByCityAndType(Guid cityId, string type);

        /// <summary>
        /// Inserts a City into the Repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Insert(EntityModel entity);
    }
}
