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
        public void BasicInsertReview()
        {
            bool passed = false;
            ReviewsDAL rd = new ReviewsDAL();
            passed = rd.InsertRestarantReview(1,1,1,"using my class? again?",1);
            Assert.IsTrue(passed);
        }

        [Test]
        public void GetAllReviewsByCity()
        {
            string city = "Sharon";
            ReviewDataContainer rdc = new ReviewDataContainer();
            ReviewsDAL rd = new ReviewsDAL();
            rd.InsertRestarantReview(1, 5, 1, "Reviews by city test", 1);
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
            rd.InsertRestarantReview(1, 5, 1, "Reviews by restaurant test", 1);
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
            rd.InsertRestarantReview(1, 5, 1, "Reviews by user test", 1);
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            reviews = rd.GetAllReviewsByUser(userName);
            rdc = reviews.FirstOrDefault();
            Console.WriteLine(rdc.ReviewText.ToString());
            Assert.IsTrue(rdc.Username == userName);
        }

        [Test, Ignore]
        public void TestLogging()
        {
            bool passed = false;
            ReviewsDAL rd = new ReviewsDAL();
            passed = rd.InsertRestarantReview(1234, 1, 1, "Should be a FK violation.", 1);
            Assert.IsTrue(passed);
        }
   
    }
}
