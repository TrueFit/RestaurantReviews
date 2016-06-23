using System;
using System.Collections.Generic;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public interface IEntityRepository
    {
        EntityModel GetByGuid(Guid guid);
        IEnumerable<EntityModel> GetByCity(Guid guid);
        void Insert(EntityModel entity);
    }
}
