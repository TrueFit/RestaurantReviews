using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truefit.Entities.Models;
using Truefit.Entities.Repositories;

namespace Truefit.Entities
{
    public class EntityService : IEntityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IEntityRepository _entityRepository;

        public EntityService(ICityRepository cityRepository, IEntityRepository entityRepository)
        {
            this._cityRepository = cityRepository;
            this._entityRepository = entityRepository;
        }

        public async Task<CityModel> GetCity(Guid guid)
        {
            return await this._cityRepository.GetByGuid(guid);
        }

        public async Task<IEnumerable<CityModel>> GetAllCities()
        {
            return await this._cityRepository.GetAll();
        }

        public async Task<EntityModel> GetEntity(Guid guid)
        {
            return await this._entityRepository.GetByGuid(guid);
        }

        public async Task<IEnumerable<EntityModel>> GetEntities(Guid cityId, string type)
        {
            type = type.ToLower();
            return await this._entityRepository.GetByCityAndType(cityId, type);
        }

        public async Task AddUserEntity(EntityModel entity)
        {
            entity.Guid = Guid.NewGuid();
            entity.Type = entity.Type?.ToLower();
            entity.IsActive = false;
            entity.NeedsReviewed = true;
            await this._entityRepository.Insert(entity);
        }
    }
}
