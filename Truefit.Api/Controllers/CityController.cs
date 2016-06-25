using System;
using System.Threading.Tasks;
using System.Web.Http;
using Truefit.Entities;

namespace Truefit.Api.Controllers
{
    [RoutePrefix("cities")]
    public class CityController : ApiController
    {
        private readonly IEntityService _entityService;

        public CityController(IEntityService entityService)
        {
            this._entityService = entityService;
        }

        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> GetAll()
        {
            return this.Ok(await this._entityService.GetAllCities());
        }

        [HttpGet]
        [Route("{cityId}/{type}")]
        public async Task<IHttpActionResult> GetEntities(Guid cityId, string type)
        {
            return this.Ok(await this._entityService.GetEntities(cityId, type));
        }
    }
}
