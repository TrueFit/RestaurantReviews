using System;
using System.Web.Http;
using RestaurantReviews.Data;

namespace RestaurantReviews.API.Controllers
{
    [RoutePrefix("api/restaurants")]
    public class RestaurantController : ApiController {

        [Route("{id:int}")]
        public IHttpActionResult GetByID(int id) {

            try {
                var r = Restaurant.GetByID(id);

                if (r == null) {
                    return NotFound();
                }

                return Ok(r);
            }
            catch (Exception x) {
                return InternalServerError(x);
            }
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(Restaurant Data) {
            try {
                var restaurant = new Restaurant() {
                    Name = Data.Name,
                    Address1 = Data.Address1,
                    Address2 = Data.Address2,
                    City = Data.City,
                    State = Data.State,
                    ZipCode = Data.ZipCode,
                    Phone = Data.Phone,
                    WebsiteURL = Data.WebsiteURL
                };

                return Ok(Restaurant.AddRestaurant(restaurant));
            }
            catch (Exception x) {
                return InternalServerError(x);
            }
        }
    }
}
