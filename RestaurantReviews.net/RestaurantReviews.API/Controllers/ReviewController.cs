using System;
using System.Web.Http;
using RestaurantReviews.API.Interfaces;
using RestaurantReviews.Data;

namespace RestaurantReviews.API.Controllers
{
    /// <summary>
    /// The Review portion of the RestaurantReviews API. 
    /// </summary>
    [RoutePrefix("api/reviews")]
    public class ReviewController : ApiController, IRestaurantApiController<Review> {

        /// <summary>
        /// Get a single review by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A review object</returns>
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
        /// Create a new review in the database
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="UserID"></param>
        /// <returns>The newly-created review</returns>
        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(Review Data, int UserID) {
            try {
                //only perform the update if the restaurant and user are both valid. 
                if (Restaurant.IsValid(Data.RestaurantID) && RestaurantReviews.Data.User.IsValid(UserID)) {
                    var review = new Review() {
                        Comments = Data.Comments,
                        CreatedDateTime = DateTime.Now,
                        IsPositive = Data.IsPositive,
                        RestaurantID = Data.RestaurantID,
                        UserID = Data.UserID
                    };

                    return Ok(Review.AddReview(review));
                }
                else {
                    return InternalServerError(new Exception(string.Format("User having ID = {0} is unable to add a review for restaurant having ID = {1}.", UserID, Data.RestaurantID)));
                }
            }
            catch (Exception x) {
                return InternalServerError(x);
            }
        }
    }
}
