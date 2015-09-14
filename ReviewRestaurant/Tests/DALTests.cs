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
        [Test]
        public void InsertRestaurantReview()
        {
            int id;
            ReviewsDAL rd = new ReviewsDAL();
            id = rd.InsertRestarantReview("MissPiggy", "Vintage Estates", "Very Good", "Nice place.");
            Assert.AreNotEqual(-1, id);
        }

        [Test]
        public void InsertReviewForNewRestaurant()
        {
            int id;
            ReviewsDAL rd = new ReviewsDAL();
            id = rd.InsertReviewForNewRestaurant("MissPiggy", "New Restaurant", "Pittsburgh","Very Good", "Nice place.");
            Assert.AreNotEqual(-1, id);
        }

        [Test]
        public void GetAllReviewsByCity()
        {
            string city = "Sharon";
            ReviewDataContainer rdc = new ReviewDataContainer();
            ReviewsDAL rd = new ReviewsDAL();
            rd.InsertReview(1, 5, 1, "Reviews by city test", 1);
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            reviews = rd.GetAllReviewsByCity(city);
            rdc = reviews.FirstOrDefault();
            Console.WriteLine(rdc.ReviewText.ToString());
            Assert.IsTrue(rdc.CityName == city);
        }

        [Test]
        public void GetAllReviewsByRestaurant()
        {
            string restaurantName = "Quaker Steak and Lube";
            ReviewDataContainer rdc = new ReviewDataContainer();
            ReviewsDAL rd = new ReviewsDAL();
            rd.InsertReview(1, 5, 1, "Reviews by restaurant test", 1);
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            reviews = rd.GetAllReviewsByRestaurant(restaurantName);
            rdc = reviews.FirstOrDefault();
            Console.WriteLine(rdc.ReviewText.ToString());
            Assert.IsTrue(rdc.RestaurantName == restaurantName);
        }

        [Test]
        public void GetAllReviewsByUser()
        {
            string userName = "MissPiggy";
            ReviewDataContainer rdc = new ReviewDataContainer();
            ReviewsDAL rd = new ReviewsDAL();
            rd.InsertReview(1, 5, 1, "Reviews by user test", 1);
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            reviews = rd.GetAllReviewsByUser(userName);
            rdc = reviews.FirstOrDefault();
            Console.WriteLine(rdc.ReviewText.ToString());
            Assert.IsTrue(rdc.Username == userName);
        }

        [Test]
        public void DeleteReview()
        {
            int id, delCount;
            ReviewsDAL rd = new ReviewsDAL();
            id = rd.InsertRestarantReview("MissPiggy", "Vintage Estates", "Very Good", "Nice place.");

            delCount = rd.DeleteReview(id, "MissPiggy");

            Assert.AreEqual(delCount, 1);
        }

        [Test]
        public void GetReviewByID()
        {
            int id;
            string reviewText = "Testing get review by ID";
            ReviewsDAL rd = new ReviewsDAL();
            id = rd.InsertRestarantReview("MissPiggy", "Vintage Estates", "Very Good", reviewText);

            ReviewDataContainer rdc = new ReviewDataContainer();
            rdc = rd.GetReviewByID(id);

            Assert.AreEqual(reviewText, rdc.ReviewText);

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

    }
}
