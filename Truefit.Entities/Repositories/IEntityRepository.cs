using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public interface IEntityRepository
    {
        Task<EntityModel> GetByGuid(Guid guid);
        Task<IEnumerable<EntityModel>> GetByCity(Guid guid);
        Task Insert(EntityModel entity);
    }
}
