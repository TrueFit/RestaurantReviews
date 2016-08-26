using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantReviews.API;
using System.Web.Http.Results;
using RestaurantReviews.Core;
using System.Web.Http;
using System.Collections.Generic;

namespace RestaurantReviews.Test
{
    [TestClass]
    public class WebAPITests
    {
        
        [TestMethod]
        public void APITest_AddNewRestaurant()
        {
            RestaurantReviewsController api = new RestaurantReviewsController();

            Restaurant newRest = new Restaurant();
            newRest.RestaurantName = "The Church Brew Works";
            newRest.Description = "Former church is now a lofty space for house-brewed beers, plus a mix of pizza, pierogi & bratwurst.";
            newRest.FoodType = "Yinzer, General";
            newRest.ServesAlcohol = true;
            newRest.KidFriendly = true;
            newRest.Street = "3525 Liberty Ave";
            newRest.City = "Pittsburgh";
            newRest.State = "PA";
            newRest.PostalCode = "15201";
            newRest.CreatorId = 1;

            IHttpActionResult result = api.CreateNewRestaurant(newRest);

            var resultObject = result as NegotiatedContentResult<object>;
            Assert.AreEqual(System.Net.HttpStatusCode.Created, resultObject.StatusCode);
            Assert.IsNotNull(resultObject.Content);
            Assert.IsTrue((int)resultObject.Content > 0);
        }

        [TestMethod]
        public void APITest_PostReview()
        {
            RestaurantReviewsController api = new RestaurantReviewsController();

            RestaurantReview review = new RestaurantReview();
            review.RestaurantId = 1;
            review.Rating = 3;
            review.WouldRecommend = true;
            review.CreatorId = 1;
            review.Comments = "Good daily specials. A little small.";

            IHttpActionResult result = api.CreateNewRestaurantReview(review);
            var resultObject = result as NegotiatedContentResult<object>;
            Assert.AreEqual(System.Net.HttpStatusCode.Created, resultObject.StatusCode);
            Assert.IsNotNull(resultObject.Content);
            Assert.IsTrue((int)resultObject.Content > 0);
        }

        [TestMethod]
        public void APITest_GetSingleRestaurantById()
        {
            RestaurantReviewsController api = new RestaurantReviewsController();
            IHttpActionResult result = api.GetRestaurantById(2);
            var resultObject = result as OkNegotiatedContentResult<object>;
            Assert.AreEqual("The Church Brew Works", ((Restaurant)resultObject.Content).RestaurantName);
            Assert.AreEqual(3, ((Restaurant)resultObject.Content).Reviews.Count);
        }

        [TestMethod]
        public void APITest_GetSingleRestaurantNoExist()
        {
            RestaurantReviewsController api = new RestaurantReviewsController();
            IHttpActionResult result = api.GetRestaurantById(100);
            var resultObject = result as OkNegotiatedContentResult<object>;
            Assert.AreEqual(null, resultObject.Content);
        }

        [TestMethod]
        public void APITest_GetUserReviews()
        {
            RestaurantReviewsController api = new RestaurantReviewsController();
            IHttpActionResult result = api.GetUserReviews(1);
            var resultObject = result as OkNegotiatedContentResult<object>;
            List<RestaurantReview> reviews = (List<RestaurantReview>)resultObject.Content;
            Assert.AreEqual(4, reviews.Count);
        }
    }
}
