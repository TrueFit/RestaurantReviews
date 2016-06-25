﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biggy.Core;
using Biggy.Data.Json;
using RestaurantReviews.Data.Models;

namespace RestaurantReviews.Data.Repositories
{
    public class EntityJsonRepository : IEntityRepository
    {
        private readonly BiggyList<EntityModel> _entities; 
        public EntityJsonRepository(JsonDbCore dbCore)
        {
            var store = new JsonStore<EntityModel>(dbCore);
            this._entities = new BiggyList<EntityModel>(store);
        }

        public async Task<EntityModel> GetByGuid(Guid guid)
        {
            return this._entities.FirstOrDefault(x => x.Guid == guid);
        }

        public async Task<IEnumerable<EntityModel>> GetByCityAndType(Guid cityId, string type)
        {
            return this._entities.Where(x => x.CityGuid == cityId && x.Type == type);
        }

        public async Task Insert(EntityModel entity)
        {
            this._entities.Add(entity);
        }
    }
}
