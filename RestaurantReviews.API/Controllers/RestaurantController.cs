using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using RestaurantReviews.API.Interfaces;
using RestaurantReviews.Data;

namespace RestaurantReviews.API.Controllers
{
    /// <summary>
    /// The Restaurant portion of the RestaurantReviews API.
    /// </summary>
    [System.Web.Http.RoutePrefix("api/restaurants")]
    public class RestaurantController : ApiController, IRestaurantApiController<Restaurant> {

        /// <summary>
        /// Get a single restaurant by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A restaurant object</returns>
        [System.Web.Http.Route("{id:int}")]
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

        /// <summary>
        /// Create a new restaurant in the database
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="UserID"></param>
        /// <returns>The newly-created restuarant</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("create")]
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

        /// <summary>
        /// Get a list of reviews for a given restaurant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns>A</returns>
        [System.Web.Http.Route("{id:int}/reviews/{order?}")]
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

        /// <summary>
        /// Get all users which have left reviews for this restaurant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.Route("{id:int}/users")]
        public IHttpActionResult GetUsers(int id) {
            if (Restaurant.IsValid(id)) {
                var idList = Restaurant.GetReviews(id).Select(r => r.UserID).Distinct();
                var users = RestaurantReviews.Data.User.GetAll().Where(u => idList.Contains(u.Id));
                return Ok(users);
            }
            else {
                return NotFound();
            }
        }

        /// <summary>
        /// Get restuarants by zip code
        /// </summary>
        /// <param name="ZipCode"></param>
        /// <returns>A list of restaurants</returns>
        [System.Web.Http.Route("byzip/{zipcode}")]
        public IHttpActionResult GetByZipCode(string ZipCode) {
            try {
                return Ok(Restaurant.GetByZipCode(ZipCode));
            }
            catch (Exception x) {
                return NotFound();
            }
        }

        /// <summary>
        /// Get restaurants by city
        /// </summary>
        /// <param name="City"></param>
        /// <returns>A list of restaurants</returns>
        [System.Web.Http.Route("bycity/{city}")]
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
