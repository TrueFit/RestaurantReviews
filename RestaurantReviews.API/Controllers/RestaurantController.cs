using System;
using System.Web.Http;
using RestaurantReviews.API.Interfaces;
using RestaurantReviews.Data;

namespace RestaurantReviews.API.Controllers
{
    [RoutePrefix("api/restaurants")]
    public class RestaurantController : ApiController, IRestaurantApiController<Restaurant> {

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
        public IHttpActionResult Create(Restaurant Data, int UserID) {
            try {
                if (RestaurantReviews.Data.User.IsValid(UserID)) {
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
                else {
                    return InternalServerError(new Exception(string.Format("User having ID = {0} is not valid. New restaurant cannot be created.", UserID)));
                }
            }
            catch (Exception x) {
                return InternalServerError(x);
            }
        }

        [Route("{id:int}/reviews/{order?}")]
        public IHttpActionResult GetReviews(int id, string order = "desc") {
            try {
                var sortReverse = order.ToLower().Trim().Equals("desc");
                var reviews = Restaurant.GetReviews(id, sortReverse);
                return Ok(reviews);
            }
            catch (Exception x) {
                return NotFound();
            }
        }

        [Route("{id:int}/users")]
        public IHttpActionResult GetUsers(int id) {
            return InternalServerError(new NotImplementedException());
        }

        [Route("byzip/{zipcode}")]
        public IHttpActionResult GetByZipCode(string ZipCode) {
            try {
                return Ok(Restaurant.GetByZipCode(ZipCode));
            }
            catch (Exception x) {
                return NotFound();
            }
        }

        [Route("bycity/{city}")]
        public IHttpActionResult GetByCity(string City) {
            try {
                return Ok(Restaurant.GetByCity(City));
            }
            catch (Exception x) {
                return NotFound();
            }
        }
    }
}
