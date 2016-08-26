using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using RestaurantReviews.Core;
using RestaurantReviews.Service;


namespace RestaurantReviews.API
{
    public class RestaurantReviewsController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetRestaurantById(int id)
        {
            RestaurantService srv = new RestaurantService();
            ServiceCallResult results = srv.GetRestaurantById(id);
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);

            return Ok(results.ResultObject);
        }

        [HttpGet]
        public IHttpActionResult GetAllRestaurants()
        {
            RestaurantService srv = new RestaurantService();
            ServiceCallResult results = srv.GetAllRestaurants();
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);
            
            return Ok(results.ResultObject);
        }

        [HttpGet]
        public IHttpActionResult GetRestaurantsInCity(string city)
        {
            RestaurantService srv = new RestaurantService();
            ServiceCallResult results = srv.GetRestaurantsForCity(city);
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);
            
            else if (results.ResultObject == null)
                return Ok();
            
            IEnumerable<Restaurant> rests = (IEnumerable<Restaurant>)results.ResultObject;
            return Ok(results.ResultObject);
        }

        [HttpGet]
        public IHttpActionResult GetRestaurantsAroundLocation(double longitude, double latitude, int distance)
        {
            RestaurantService srv = new RestaurantService();
            ServiceCallResult results = srv.GetRestaurantsAroundMe(distance, longitude, latitude);
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);
            else if (results.ResultObject == null)
                return Ok();

            IEnumerable<Restaurant> rests = (IEnumerable<Restaurant>)results.ResultObject;
            return Ok(results.ResultObject);
        }

        [HttpPost]
        public IHttpActionResult CreateNewRestaurant([FromBody] Restaurant newRestaurant)
        {
            RestaurantService srv = new RestaurantService();
            newRestaurant.CreatorId = 1;
            ServiceCallResult results = srv.AddNewRestaurant(newRestaurant);
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);

            //this will return Id for the created restaurant
            return Content(HttpStatusCode.Created, results.ResultObject);

        }

        [HttpPost]
        public IHttpActionResult CreateNewRestaurantReview([FromBody] RestaurantReview newReview)
        {
            RestaurantService srv = new RestaurantService();
            newReview.CreatorId = 1;
            ServiceCallResult results = srv.CreateRestaurantReview(newReview);
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);

            //this will return Id for the created restaurant review
            return Content(HttpStatusCode.Created, results.ResultObject);
        }

        [HttpGet]
        public IHttpActionResult DeleteUserReview(int id)
        {
            UserService srv = new UserService();
            ServiceCallResult results = srv.DeleteReview(id);
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);

            //this will return Id for the created restaurant review
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetUserReviews(int id)
        {
            UserService srv = new UserService();
            ServiceCallResult results = srv.GetUserReviews(id);
            if (results.ValidationErrors.Count > 0)
                return Content(HttpStatusCode.BadRequest, results.ValidationErrors);
            else if (results.ResultObject == null)
                return Ok();

            IEnumerable<RestaurantReview> rests = (IEnumerable<RestaurantReview>)results.ResultObject;
            return Ok(results.ResultObject);
        }
    }
}