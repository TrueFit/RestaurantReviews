using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;


namespace BL
{
    /// <summary>
    /// Basic BL class with simple validation
    /// </summary>
    public class Review
    {
        private IReviewsDAL dal = new ReviewsDAL();

        /// <summary>
        /// Creates basic review from data passed in, simple validation
        /// </summary>
        /// <param name="username"></param>
        /// <param name="restaurantName"></param>
        /// <param name="ratingDescription"></param>
        /// <param name="reviewText"></param>
        /// <returns></returns>
        public int CreateReview(string username, string restaurantName, string ratingDescription, string reviewText)
        {
            int id = -1;
            try
            {
                ReviewDataContainer rdc = new ReviewDataContainer();
                rdc.Username = username;
                rdc.RestaurantName = restaurantName;
                rdc.RatingDescription = ratingDescription;
                rdc.ReviewText = reviewText;
      
                if(ValidReview(rdc))
                {
                    id = dal.InsertRestarantReview(rdc);
                }
                else
                {
                    //Go back to caller and do error message things.
                    Console.WriteLine("Kick out!");
                }
                
            }
            catch (Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }
            return id;
        }

        /// <summary>
        /// Creates review for new restaurant, simple validation
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="restaurantName"></param>
        /// <param name="cityName"></param>
        /// <param name="ratingDescription"></param>
        /// <param name="reviewText"></param>
        /// <returns></returns>
        public int CreateReviewForNewRestaurant(string userName, string restaurantName, string cityName,
            string ratingDescription, string reviewText)
        {  
            int id = -1;
            try
            {
                ReviewDataContainer rdc = new ReviewDataContainer();
                rdc.Username = userName;
                rdc.RestaurantName = restaurantName;
                rdc.RatingDescription = ratingDescription;
                rdc.ReviewText = reviewText;
                rdc.CityName = cityName;

                if (ValidReview(rdc))
                {
                    id = dal.InsertReviewForNewRestaurant(rdc);
                }
                else
                {
                    //Go back to caller and do error message things.
                    Console.WriteLine("Kick out!");
                }
            }
            catch (Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }
            return id;
        }

        /// <summary>
        /// Get individual review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public ReviewDataContainer GetReview(int reviewId)
        {
            ReviewDataContainer rdc = new ReviewDataContainer();
            try
            {
                rdc = dal.GetReviewByID(reviewId);
            }
            catch(Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return rdc;
        }

        /// <summary>
        /// Get all reviews for a restaurant
        /// </summary>
        /// <param name="restaurantName"></param>
        /// <returns></returns>
        public IList<ReviewDataContainer> GetReviewsForRestaurant(string restaurantName)
        {
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            try
            {
                reviews = dal.GetAllReviewsByRestaurant(restaurantName);
            }
            catch (Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }
            return reviews;
        }

        /// <summary>
        /// Get all reviews for a user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<ReviewDataContainer> GetReviewsForUser(string username)
        {
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            try
            {
                reviews = dal.GetAllReviewsByUser(username);
            }
            catch (Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }
            return reviews;
        }

        /// <summary>
        /// Get all reviews for a city
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public IList<ReviewDataContainer> GetReviewsForCity(string cityName)
        {
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            try
            {
                reviews = dal.GetAllReviewsByCity(cityName);
            }
            catch (Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }
            return reviews;
        }

        /// <summary>
        /// Delete a review by id for a particular user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int DeleteReview(int id, string username)
        {
            int delCount;
            try
            {
                delCount = dal.DeleteReview(id, username);
            }
            catch (Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }
            return delCount;
        }

        /// <summary>
        /// basic validation
        /// </summary>
        /// <param name="rdc"></param>
        /// <returns></returns>
        private bool ValidReview(ReviewDataContainer rdc)
        {
            ValidationContext vc = new ValidationContext(rdc, null, null);
            List<ValidationResult> vResults = new List<ValidationResult>();
            return Validator.TryValidateObject(rdc, vc, vResults, true);
        }
    }
}
