using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantReviews.Core;
using RestaurantReviews.Service;
using System.Collections.Generic;

namespace RestaurantReviews.Test
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void ServiceTest_GetAllUsers()
        {
            UserService srv = new UserService();
            ServiceCallResult results = srv.GetAllUsers();
            Assert.AreEqual(0, results.ValidationErrors.Count);
            List<User> users = (List<User>)results.ResultObject;
            Assert.AreEqual(1, users.Count);
        }

        [TestMethod]
        public void ServiceTest_GetUser()
        {
            UserService srv = new UserService();
            ServiceCallResult results = srv.GetUserByEmail("admin@restaurantreviews.com");
            Assert.AreEqual(0, results.ValidationErrors.Count);
            User usr = (User)results.ResultObject;
            Assert.AreEqual("System", usr.FirstName);
        }

        [TestMethod]
        public void ServiceTest_SimpleUpdateUser()
        {
            UserService srv = new UserService();
            ServiceCallResult results = srv.GetUserByEmail("admin@restaurantreviews.com");
            Assert.AreEqual(0, results.ValidationErrors.Count);
            User usr = (User)results.ResultObject;
            Assert.AreEqual("System", usr.FirstName);
            usr.LastName = "Administrator Jr";
            ServiceCallResult results2 = srv.UpdateUser(usr);
            Assert.AreEqual(0, results.ValidationErrors.Count);
            Assert.AreEqual("Administrator Jr",usr.LastName);
        }

        [TestMethod]
        public void ServiecTest_AddNewRestaurant()
        {
            RestaurantService srv = new RestaurantService();
            Restaurant newRest = new Restaurant();
            newRest.RestaurantName = "Aliota's";
            newRest.Description = "Hole in the wall bar and lounge. Daily specials.";
            newRest.FoodType = "Italian, General";
            newRest.ServesAlcohol = true;
            newRest.KidFriendly = true;
            newRest.Street = "17 Grant Ave";
            newRest.City = "Etna";
            newRest.State = "PA";
            newRest.PostalCode = "15223";
            newRest.CreatorId = 1;
            ServiceCallResult result = srv.AddNewRestaurant(newRest);
            Assert.AreEqual(0, result.ValidationErrors.Count);
        }

        [TestMethod]
        public void ServiceTest_RestaurantsAroundMe()
        {
            RestaurantService srv = new RestaurantService();

            double myLat = 40.500304;
            double myLong = -79.944715;
            int distance = 1;

            ServiceCallResult result = srv.GetRestaurantsAroundMe(distance, myLong, myLat);
            ///Assert.AreEqual(result.ValidationErrors.Count, 0);

            List<Restaurant> rests = (List<Restaurant>)result.ResultObject;
            Assert.AreEqual(1, rests.Count);

        }
    }
}
