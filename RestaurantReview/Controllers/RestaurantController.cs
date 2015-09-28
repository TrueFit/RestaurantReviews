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
using RestaurantReview.Models.RestaurantModels;
using RestaurantReview.Utilities;

namespace RestaurantReview.Controllers
{
    public class RestaurantController : ApiController
    {
        private RestRevEntities db = new RestRevEntities();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        // GET api/Restaurant
        // Search for restaurants by given criteria in SearchRestaurantModel
        [HttpGet]
        public IEnumerable<DisplayRestaurantModel> Select([FromUri]SearchRestaurantModel restaurant)
        {
            IQueryable<Restaurant> filteredRestaurants = db.Restaurants;
            List<DisplayRestaurantModel> displayRestaurants = new List<DisplayRestaurantModel>();

            if (restaurant != null)
            {
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
                    filteredRestaurants = filteredRestaurants.Where(r => r.Tags.Select(t => t.TagName).Contains(restaurant.Tag));
                }

                // Order the restaurants by the OrderBy and Order specified
                filteredRestaurants = OrderRestaurants(filteredRestaurants, restaurant.OrderBy, restaurant.Order);

                // Filter restaurants by quantity requested and page number
                filteredRestaurants = restaurant.GetPage(filteredRestaurants);
            }

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
        // Allows a restaurant owner to update their restaurant
        [AuthorizeMembership]
        [HttpPut]
        public IHttpActionResult PutRestaurant(int id, UpdateRestaurantModel restaurantModel)
        {
            string username = SessionUtilities.GetUserName(Request);

            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (restaurantModel == null || id != restaurantModel.Id || !RestaurantExists(id, username))
            {
                return BadRequest();
            }
            restaurantModel.OwnerUserName = username;
            
            // Update the database
            Restaurant restaurant = db.Restaurants.Find(restaurantModel.Id);
            restaurant.Name = restaurantModel.Name;
            restaurant.City = restaurantModel.City;
            restaurant.State = restaurantModel.State;
            restaurant.Zipcode = restaurantModel.Zipcode;
            restaurant.StreetAddress1 = restaurantModel.StreetAddress1;
            restaurant.StreetAddress2 = restaurantModel.StreetAddress2;
            restaurant.PhoneNum = restaurantModel.PhoneNum;
            db.Entry(restaurant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Unable to update restaurant");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Restaurant
        // Allows a user to add a new restaurant
        [AuthorizeMembership]
        [ResponseType(typeof(DisplayRestaurantModel))]
        [HttpPost]
        public IHttpActionResult PostRestaurant(CreateRestaurantModel restaurantModel)
        {
            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (restaurantModel == null)
            {
                return BadRequest();
            }

            // Create restaurant
            restaurantModel.OwnerUserName = SessionUtilities.GetUserName(Request);
            Restaurant restaurant = Mapper.Map<Restaurant>(restaurantModel);
            db.Restaurants.Add(restaurant);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Unable to create restaurant");
            }
            return CreatedAtRoute("DefaultApi", new { id = restaurant.Id }, Mapper.Map<DisplayRestaurantModel>(restaurant));
        }

        // DELETE api/Restaurant/5
        [AuthorizeMembership]
        [ResponseType(typeof(DisplayRestaurantModel))]
        [HttpPost]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null || !RestaurantExists(id, SessionUtilities.GetUserName(Request)))
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