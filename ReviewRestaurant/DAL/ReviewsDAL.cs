using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;
using System.Collections;
using System.ComponentModel.DataAnnotations;


namespace DAL
{
    /// <summary>
    /// DAL class for inserting, getting and deleting restaurant reviews
    /// </summary>
    public class ReviewsDAL : IReviewsDAL
    {
        DBConnection connection = new DBConnection();
        /// <summary>
        /// Insert basic restaurant review and returns the id of the new review
        /// </summary>
        /// <param name="rdc"></param>
        /// <returns></returns>
        public int InsertRestarantReview(ReviewDataContainer rdc)
        {
            int id = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertRestaurantReview", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter reviewID = new SqlParameter("@ReviewID", SqlDbType.Int);
                    reviewID.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = rdc.Username;
                    cmd.Parameters.Add("@RestaurantName", SqlDbType.NVarChar).Value = rdc.RestaurantName;
                    cmd.Parameters.Add("@RatingDescription", SqlDbType.NVarChar).Value = rdc.RatingDescription;
                    cmd.Parameters.Add("@ReviewText", SqlDbType.NVarChar).Value = rdc.ReviewText;
                    cmd.Parameters.Add(reviewID);
                    
                    connection.ExecNonQuery(cmd);
                    id = Int16.Parse(reviewID.Value.ToString());
                }
            }
            catch (System.Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return id;
        }

        /// <summary>
        /// Inserts a review for a new restaurant and returns the id of the new review
        /// </summary>
        /// <param name="rdc"></param>
        /// <returns></returns>
        public int InsertReviewForNewRestaurant(ReviewDataContainer rdc)
        {
            int id = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertReviewForNewRestaurant", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter reviewID = new SqlParameter("@ReviewID", SqlDbType.Int);
                    reviewID.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = rdc.Username;
                    cmd.Parameters.Add("@NewRestaurantName", SqlDbType.NVarChar).Value = rdc.RestaurantName;
                    cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = rdc.CityName;
                    cmd.Parameters.Add("@RatingDescription", SqlDbType.NVarChar).Value = rdc.RatingDescription;
                    cmd.Parameters.Add("@ReviewText", SqlDbType.NVarChar).Value = rdc.ReviewText;
                    cmd.Parameters.Add(reviewID);

                    connection.ExecNonQuery(cmd);
                    id = Int16.Parse(reviewID.Value.ToString());
                }
            }
            catch (System.Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return id;
        }

        /// <summary>
        /// Gets all restaurant reviews for a city
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public IList<ReviewDataContainer> GetAllReviewsByCity(string cityName)
        {
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAllReviewsByCity", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = cityName;
                    
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ReviewDataContainer rdc = new ReviewDataContainer();
                            rdc.ReviewID = Int16.Parse(dr["ReviewID"].ToString());
                            rdc.RestaurantName = dr["RestaurantName"].ToString();
                            rdc.Username = dr["Username"].ToString();
                            rdc.RatingDescription = dr["RatingDescription"].ToString();
                            rdc.ReviewText = dr["ReviewText"].ToString();
                            rdc.CityName = dr["CityName"].ToString();
                            rdc.StateName = dr["StateName"].ToString();
                            rdc.ReviewDate = DateTime.Parse(dr["ReviewDate"].ToString());

                            reviews.Add(rdc);
                        }
                        dr.Close();
                    }
                    
                }
            }
            catch (System.Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return reviews;
        }

        /// <summary>
        /// Gets all reviews for a restaurant
        /// </summary>
        /// <param name="restaurantName"></param>
        /// <returns></returns>
        public IList<ReviewDataContainer> GetAllReviewsByRestaurant(string restaurantName)
        {
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAllReviewsByRestaurant", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RestaurantName", SqlDbType.NVarChar).Value = restaurantName;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ReviewDataContainer rdc = new ReviewDataContainer();
                            rdc.ReviewID = Int16.Parse(dr["ReviewID"].ToString());
                            rdc.RestaurantName = dr["RestaurantName"].ToString();
                            rdc.Username = dr["Username"].ToString();
                            rdc.RatingDescription = dr["RatingDescription"].ToString();
                            rdc.ReviewText = dr["ReviewText"].ToString();
                            rdc.CityName = dr["CityName"].ToString();
                            rdc.StateName = dr["StateName"].ToString();
                            rdc.ReviewDate = DateTime.Parse(dr["ReviewDate"].ToString());

                            reviews.Add(rdc);
                        }
                        dr.Close();
                    }

                }
            }
            catch (System.Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return reviews;
        }

        /// <summary>
        /// Gets all reviews for a given user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IList<ReviewDataContainer> GetAllReviewsByUser(string userName)
        {
            IList<ReviewDataContainer> reviews = new List<ReviewDataContainer>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAllReviewsByUser", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ReviewDataContainer rdc = new ReviewDataContainer();
                            rdc.ReviewID = Int16.Parse(dr["ReviewID"].ToString());
                            rdc.RestaurantName = dr["RestaurantName"].ToString();
                            rdc.Username = dr["Username"].ToString();
                            rdc.RatingDescription = dr["RatingDescription"].ToString();
                            rdc.ReviewText = dr["ReviewText"].ToString();
                            rdc.CityName = dr["CityName"].ToString();
                            rdc.StateName = dr["StateName"].ToString();
                            rdc.ReviewDate = DateTime.Parse(dr["ReviewDate"].ToString());

                            reviews.Add(rdc);
                        }
                        dr.Close();
                    }

                }
            }
            catch (System.Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return reviews;
        }

        /// <summary>
        /// Deletes a review for a user for a given id
        /// </summary>
        /// <param name="reviewID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int DeleteReview(int reviewID, string userName)
        {
            int delCount = -1;
            try
            {
                using (SqlCommand cmd = new SqlCommand("DeleteReviewByID", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter deleteCount = new SqlParameter("@DeleteCount", SqlDbType.Int);
                    deleteCount.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ReviewID", SqlDbType.Int).Value = reviewID;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;
                    cmd.Parameters.Add(deleteCount);
                    connection.ExecNonQuery(cmd);

                    delCount = Int16.Parse(deleteCount.Value.ToString());
                }
            }
            catch (System.Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return delCount;
        }

        /// <summary>
        /// Get a single review
        /// </summary>
        /// <param name="reviewID"></param>
        /// <returns></returns>
        public ReviewDataContainer GetReviewByID(int reviewID)
        {
            ReviewDataContainer rdc = new ReviewDataContainer();
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetReviewByID", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReviewID", SqlDbType.Int).Value = reviewID;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            rdc.ReviewID = Int16.Parse(dr["ReviewID"].ToString());
                            rdc.RestaurantName = dr["RestaurantName"].ToString();
                            rdc.Username = dr["Username"].ToString();
                            rdc.RatingDescription = dr["RatingDescription"].ToString();
                            rdc.ReviewText = dr["ReviewText"].ToString();
                            rdc.CityName = dr["CityName"].ToString();
                            rdc.StateName = dr["StateName"].ToString();
                            rdc.ReviewDate = DateTime.Parse(dr["ReviewDate"].ToString());

                        }
                        dr.Close();
                    }
                }
            }
            catch (System.Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return rdc;
        }

        /// <summary>
        /// Used for testing purposes.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="restaurantID"></param>
        /// <param name="ratingID"></param>
        /// <param name="reviewText"></param>
        /// <param name="reviewID"></param>
        /// <returns></returns>
        public bool InsertReview(int userID, int restaurantID, int ratingID, string reviewText, int reviewID)
        {
            bool inserted = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertReview", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                    cmd.Parameters.Add("@RestaurantID", SqlDbType.Int).Value = restaurantID;
                    cmd.Parameters.Add("@RatingID", SqlDbType.Int).Value = ratingID;
                    cmd.Parameters.Add("@ReviewText", SqlDbType.NVarChar).Value = reviewText;
                    cmd.Parameters.Add("@ReviewID", SqlDbType.Int).Value = reviewID;
                    connection.ExecNonQuery(cmd);

                    inserted = true;
                }
            }
            catch (System.Exception e)
            {
                if (e is ValidationException)
                {
                    Console.WriteLine(e);
                }

                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

            return inserted;
        }
    }
}
