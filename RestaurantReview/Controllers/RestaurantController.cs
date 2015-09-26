using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestaurantReview.Models;
using System.Security.Principal;
using System.Web.Security;
using RestaurantReview.Filters;
using RestaurantReview.Models.CustomRestRevModels;
using System.Diagnostics;

namespace RestaurantReview.Controllers
{
    public class RestaurantController : ApiController
    {
        private RestRevEntities db = new RestRevEntities();

        // GET api/Restaurant
        [HttpGet]
        public IEnumerable<Restaurant> Select([FromUri]RestaurantSearchModel restaurant)
        {
            IQueryable<Restaurant> filteredRestaurants = db.Restaurants;

            // Filter the restaurants
            if (!String.IsNullOrWhiteSpace(restaurant.Name))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.Name.Contains(restaurant.Name));
            }

            if (!String.IsNullOrWhiteSpace(restaurant.City))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.City.Equals(restaurant.City));
            }

            if (!String.IsNullOrWhiteSpace(restaurant.State))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.State.Equals(restaurant.State));
            }

            if (!String.IsNullOrWhiteSpace(restaurant.StreetAddress1))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.StreetAddress1.Equals(restaurant.StreetAddress1));
            }

            if (!String.IsNullOrWhiteSpace(restaurant.StreetAddress2))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.StreetAddress2.Equals(restaurant.StreetAddress2));
            }

            if (!String.IsNullOrWhiteSpace(restaurant.Zipcode))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.Zipcode.Equals(restaurant.Zipcode));
            }

            if (!String.IsNullOrWhiteSpace(restaurant.Tag))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.Tags.Contains(new Tag() { TagName = restaurant.Tag }));
            }

            // Order the restaurants by the OrderBy and Order specified
            if (!String.IsNullOrWhiteSpace(restaurant.OrderBy) && restaurant.GetType().GetProperty(restaurant.OrderBy) != null)
            {
                filteredRestaurants = OrderRestaurants(filteredRestaurants, restaurant.OrderBy, restaurant.Order);
            }
            else
            {
                // Order by Id by default explicitly in order to be able to call Skip() to determine number of restaurants returned
                filteredRestaurants = filteredRestaurants.OrderBy(r => r.Id);
            }

            // Filter restaurants by quantity requested and page number
            if (restaurant.NumRestaurants > 0)
            {
                restaurant.PageNum = restaurant.PageNum <= 0 ? 0 : restaurant.PageNum - 1;
                filteredRestaurants = filteredRestaurants
                                        .Skip(restaurant.PageNum * restaurant.NumRestaurants)
                                        .Take(restaurant.NumRestaurants);
            }

            return filteredRestaurants;
        }

        // GET api/Restaurant/5
        // Returns a restaurant with the given id
        [ResponseType(typeof(Restaurant))]
        [HttpGet]
        public IHttpActionResult GetRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        // PUT api/Restaurant/5
        [AuthorizeMembership]
        [HttpPut]
        public IHttpActionResult PutRestaurant(int id, Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurant.Id)
            {
                return BadRequest("The restaurant requested for update does not match the id given");
            }
            
            // Ensure one user cannot update a restaurant owned by another user
            restaurant.OwnerUserName = GetUserName(Request);
            if (!RestaurantExists(id, restaurant.OwnerUserName))
            {
                return NotFound();
            }

            db.Entry(restaurant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Restaurant
        [AuthorizeMembership]
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        public IHttpActionResult PostRestaurant(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            restaurant.OwnerUserName = GetUserName(Request);
            db.Restaurants.Add(restaurant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurant.Id }, restaurant);
        }

        // DELETE api/Restaurant/5
        [AuthorizeMembership]
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            string currUserName = GetUserName(Request);
            Restaurant restaurant = db.Restaurants.Where(r => r.Id == id && r.OwnerUserName == currUserName).FirstOrDefault();
            if (restaurant == null)
            {
                return NotFound();
            }

            db.Restaurants.Remove(restaurant);
            db.SaveChanges();

            return Ok(restaurant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetUserName(HttpRequestMessage request)
        {
            return request.GetRouteData().Values["MemberUserName"].ToString();
        }

        private bool RestaurantExists(int id, string username = null)
        {
            return username == null ?
                db.Restaurants.Count(e => e.Id == id) > 0 :
                db.Restaurants.Count(e => e.Id == id && e.OwnerUserName.Equals(username)) > 0;
        }

        /* Orders a restaurants query by the following fields
         * - Name
         * - City
         * - State
         * - Zipcode
         * - Number of reviews (NumReviews)
         */
        private IQueryable<Restaurant> OrderRestaurants(IQueryable<Restaurant> restaurants, string orderby, string order)
        {
            if (orderby.ToLower() == "name")
            {
                restaurants = order != null && order.ToLower() == "desc" ? 
                    restaurants.OrderByDescending(r => r.Name) :
                    restaurants.OrderBy(r => r.Name);
            }
            else if (orderby.ToLower() == "city")
            {
                restaurants = order != null && order.ToLower() == "desc" ?
                    restaurants.OrderByDescending(r => r.City) :
                    restaurants.OrderBy(r => r.City);
            }
            else if (orderby.ToLower() == "state")
            {
                restaurants = order != null && order.ToLower() == "desc" ?
                    restaurants.OrderByDescending(r => r.State) :
                    restaurants.OrderBy(r => r.State);
            }
            else if (orderby.ToLower() == "zipcode")
            {
                restaurants = order != null && order.ToLower() == "desc" ?
                    restaurants.OrderByDescending(r => r.Zipcode) :
                    restaurants.OrderBy(r => r.Zipcode);
            }
            else if (orderby.ToLower() == "numreviews")
            {
                restaurants = order != null && order.ToLower() == "desc" ?
                    restaurants.OrderByDescending(r => r.Reviews.Count()) :
                    restaurants.OrderBy(r => r.Reviews.Count());
            }

            return restaurants;
        }
    }
}