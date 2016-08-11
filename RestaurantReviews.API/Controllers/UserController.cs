using System;
using System.Web.Http;
using RestaurantReviews.API.Interfaces;
using RestaurantReviews.Data;

namespace RestaurantReviews.API.Controllers
{
    /// <summary>
    /// The User portion of the RestaurantReviews API. 
    /// </summary>
    [RoutePrefix("api/users")]
    public class UserController : ApiController, IRestaurantApiController<User> {

        /// <summary>
        /// Get a single user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A user object</returns>
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

        /// <summary>
        /// Create a new user in the database
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="UserID"></param>
        /// <returns>The newly-created user</returns>
        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(User Data, int UserID) {
            try {
                //only perform the update if the restaurant and user are both valid. 
                if (RestaurantReviews.Data.User.IsValid(UserID)) {
                    var user = new RestaurantReviews.Data.User() {
                        Locked = Data.Locked,
                        Username = Data.Username
                    };

                    return Ok(RestaurantReviews.Data.User.AddUser(user));
                }
                else {
                    return InternalServerError(new Exception(string.Format("User having ID = {0} is not valid. Unable to add a new user.", UserID)));
                }
            }
            catch (Exception x) {
                return InternalServerError(x);
            }
        }

        /// <summary>
        /// Get a list of reviews left by the user
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>A list of reviews</returns>
        [Route("{id:int}/reviews")]
        public IHttpActionResult GetReviews(int UserID) {
            try {
                if (RestaurantReviews.Data.User.IsValid(UserID)) {
                    return Ok(RestaurantReviews.Data.User.GetReviews(UserID));
                }
                else {
                    return InternalServerError(new Exception("User having ID = {0} is invalid or does not exist. No reviews could be retrieved."));
                }
            }
            catch (Exception x) {
                return InternalServerError(x);
            }
        }
    }
}
