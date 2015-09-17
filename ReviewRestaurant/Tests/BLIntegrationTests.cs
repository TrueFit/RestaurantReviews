using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BL;
using DAL;


namespace Tests
{
    [TestFixture]
    class BLIntegrationTests
    {
        private Review reviewBL = new Review();
        private string username = "KermitTheFrog";
        private string restaurantName = "Nakama";
        private string excellentReview = "Excellent";
        private string cityName = "Pittsburgh";
        private string reviewText = "Review from BL integration test";
        private string toolong = "aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeeefff";
        private string newRestaurant = "A new place";


        [Test, Category("BLIntegration")]
        public void BLCreateReview()
        {
            int id = -1;
            id = reviewBL.CreateReview(username, restaurantName, excellentReview, reviewText);
            Assert.AreNotEqual(-1, id);
        }

        [Test, Category("BLIntegration")]
        public void BLCreateReviewForNewRestaurant()
        {
            int id = -1;
            id = reviewBL.CreateReviewForNewRestaurant(username, newRestaurant, cityName, excellentReview, reviewText);
            Assert.AreNotEqual(-1, id);
        }

        [Test, Category("BLIntegration")]
        public void BLTestValidator()
        {
            int id = -1;
            id = reviewBL.CreateReview(toolong, restaurantName, excellentReview, reviewText);
            Assert.AreEqual(-1, id);
        }

        [Test, Category("BLIntegration")]
        public void BLDeleteReview()
        {
            int id = -1;
            int deleteCount = -1;
            id = reviewBL.CreateReview(username, restaurantName, excellentReview, reviewText);
            deleteCount = reviewBL.DeleteReview(id, username);
            Assert.AreNotEqual(-1, deleteCount);
        }

        [Test, Category("BLIntegration")]
        public void BLGetReviewsByUser()
        {
            reviewBL.CreateReview(username, restaurantName, excellentReview, reviewText);
            IList <ReviewDataContainer> reviews = reviewBL.GetReviewsForUser(username);
            ReviewDataContainer rdc = reviews.FirstOrDefault();
            Assert.AreEqual(username, rdc.Username);
        }

        [Test, Category("BLIntegration")]
        public void BLGetReviewsByCity()
        {
            reviewBL.CreateReview(username, restaurantName, excellentReview, reviewText);
            IList<ReviewDataContainer> reviews = reviewBL.GetReviewsForCity(cityName);
            ReviewDataContainer rdc = reviews.FirstOrDefault();
            Assert.AreEqual(cityName, rdc.CityName);
        }

    }
}
