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
using System.Web.Script.Serialization;
using AutoMapper;

namespace RestaurantReview.Controllers
{
    public class RestaurantController : ApiController
    {
        private RestRevEntities db = new RestRevEntities();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        // GET api/Restaurant
        [HttpGet]
        public IEnumerable<DisplayRestaurantModel> Select([FromUri]SearchRestaurantModel restaurant)
        {
            IQueryable<Restaurant> filteredRestaurants = db.Restaurants;
            List<DisplayRestaurantModel> displayRestaurants = new List<DisplayRestaurantModel>();

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
            filteredRestaurants = OrderRestaurants(filteredRestaurants, restaurant.OrderBy, restaurant.Order);

            // Filter restaurants by quantity requested and page number
            filteredRestaurants = restaurant.GetPage(filteredRestaurants);

            foreach (Restaurant rest in filteredRestaurants)
            {
                displayRestaurants.Add(Mapper.Map<DisplayRestaurantModel>(rest));
            }

            return displayRestaurants;
        }

        // GET api/Restaurant/5
        // Returns a restaurant with the given id
        [ResponseType(typeof(DisplayRestaurantModel))]
        [HttpGet]
        public IHttpActionResult GetRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<DisplayRestaurantModel>(restaurant));
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
            
            // Ensure a user can't update a restaurant owned by another user
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
        [ResponseType(typeof(DisplayRestaurantModel))]
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

            return CreatedAtRoute("DefaultApi", new { id = restaurant.Id }, Mapper.Map<DisplayRestaurantModel>(restaurant));
        }

        // DELETE api/Restaurant/5
        [AuthorizeMembership]
        [ResponseType(typeof(DisplayRestaurantModel))]
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

            return Ok(Mapper.Map<DisplayRestaurantModel>(restaurant));
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
         * - Lowest review rating (MinRating)
         * - Average review rating (AvgRating)
         */
        private IQueryable<Restaurant> OrderRestaurants(IQueryable<Restaurant> restaurants, string orderby, string order)
        {
            restaurants = restaurants.OrderBy(rest => rest.Id);
            if (!String.IsNullOrWhiteSpace(orderby))
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
                else if (orderby.ToLower() == "minrating")
                {
                    restaurants = order != null && order.ToLower() == "desc" ?
                        restaurants.OrderByDescending(rest => rest.Reviews.Select(rev => rev.Rating).Min()) :
                        restaurants.OrderBy(rest => rest.Reviews.Select(rev => rev.Rating).Min());
                }
                else if (orderby.ToLower() == "avgrating")
                {
                    restaurants = order != null && order.ToLower() == "desc" ?
                        restaurants.OrderByDescending(rest => rest.Reviews.Select(rev => rev.Rating).Average()) :
                        restaurants.OrderBy(rest => rest.Reviews.Select(rev => rev.Rating).Average());
                }
            }

            return restaurants;
        }
    }
}