using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DAL;


namespace Tests
{   
    [TestFixture]
    public class DALTests
    {
        private string username = "MissPiggy";
        private string restaurantN = "Nakama";
        private string restaurantQSL = "Quaker Steak and Lube";
        private string ratingVG = "Very Good";
        private string ratingP = "Poor";
        private string cityPitt = "Pittsburgh";
        private string citySharon = "Sharon";
        

        [Test]
        public void InsertRestaurantReview()
        {
            int id;
            ReviewsDAL rd = new ReviewsDAL();
            id = rd.InsertRestarantReview(LoadRDCForTest("Insert Restaurant Review"));
            Assert.AreNotEqual(-1, id);
        }

        [Test]
        public void InsertReviewForNewRestaurant()
        {
            int id;
            ReviewsDAL rd = new ReviewsDAL();
            id = rd.InsertReviewForNewRestaurant(LoadRDCForNewRestaurantTest("New restaurant test"));
            Assert.AreNotEqual(-1, id);
        }

        [Test]
        public void GetAllReviewsByCity()
        {
            ReviewsDAL rd = new ReviewsDAL();
            ReviewDataContainer inRdc = LoadRDCForTest("Get all reviews by city");
            rd.InsertRestarantReview(inRdc);
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            reviews = rd.GetAllReviewsByCity(inRdc.CityName);
            ReviewDataContainer rdc = reviews.FirstOrDefault();
            Console.WriteLine(rdc.ReviewText.ToString());

            Assert.IsTrue(rdc.CityName == inRdc.CityName);
        }

        [Test]
        public void GetAllReviewsByRestaurant()
        {
            ReviewsDAL rd = new ReviewsDAL();
            ReviewDataContainer inRdc = LoadRDCForTest("Get all reviews by Restaurant");
            rd.InsertRestarantReview(inRdc);
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            reviews = rd.GetAllReviewsByRestaurant(inRdc.RestaurantName);
            ReviewDataContainer rdc = reviews.FirstOrDefault();
            Console.WriteLine(rdc.ReviewText.ToString());

            Assert.IsTrue(rdc.RestaurantName == inRdc.RestaurantName);
        }

        [Test]
        public void GetAllReviewsByUser()
        {
            ReviewsDAL rd = new ReviewsDAL();
            ReviewDataContainer inRdc = LoadRDCForTest("Get all reviews by user");
            rd.InsertRestarantReview(inRdc);
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            reviews = rd.GetAllReviewsByUser(inRdc.Username);
            ReviewDataContainer rdc = reviews.FirstOrDefault();
            Console.WriteLine(rdc.ReviewText.ToString());

            Assert.IsTrue(rdc.Username == inRdc.Username);
        }

        [Test]
        public void DeleteReview()
        {
            int id, delCount;
            ReviewsDAL rd = new ReviewsDAL();
            id = rd.InsertRestarantReview(LoadRDCForTest("Delete Review"));
            delCount = rd.DeleteReview(id, username);

            Assert.AreEqual(delCount, 1);
        }

        [Test]
        public void GetReviewByID()
        {
            int id;
            ReviewsDAL rd = new ReviewsDAL();
            ReviewDataContainer inRdc = LoadRDCForTest("Get review by ID");
            id = rd.InsertRestarantReview(inRdc);
            ReviewDataContainer rdc = new ReviewDataContainer();
            rdc = rd.GetReviewByID(id);

            Assert.AreEqual(inRdc.ReviewText, rdc.ReviewText);

        }

        [Test, Ignore]
        public void TestLogging()
        {
            bool passed = true;
            ReviewsDAL rd = new ReviewsDAL();
            passed = rd.InsertReview(1234, 1, 1, "Should be a FK violation.", 1);
            Assert.IsTrue(passed);
        }

        [Test, Ignore]
        public void BasicInsertReview()
        {
            bool passed = false;
            ReviewsDAL rd = new ReviewsDAL();
            passed = rd.InsertReview(1, 1, 1, "using my class? again?", 1);
            Assert.IsTrue(passed);
        }

        private ReviewDataContainer LoadRDCForTest(string testString)
        {
            ReviewDataContainer rdc = new ReviewDataContainer();

            rdc.Username = username;
            rdc.RestaurantName = restaurantQSL;
            rdc.RatingDescription = ratingVG;
            rdc.ReviewText = testString;
            rdc.CityName = citySharon;

            return rdc;
        }

        private ReviewDataContainer LoadRDCForNewRestaurantTest(string testString)
        {
            ReviewDataContainer rdc = new ReviewDataContainer();

            rdc.Username = username;
            rdc.RestaurantName = "Brand new place";
            rdc.RatingDescription = ratingVG;
            rdc.ReviewText = testString;
            rdc.CityName = cityPitt;

            return rdc;
        }
    }
}
