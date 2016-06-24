using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Truefit.Api.Controllers
{
    [RoutePrefix("cities")]
    public class CityController : ApiController
    {
        [HttpGet]
        [Route("{cityId}/{type}")]
        public async Task<IHttpActionResult> GetEntities(Guid cityId, string type)
        {
            return this.Ok(cityId + " " + type);
        }
    }
}
