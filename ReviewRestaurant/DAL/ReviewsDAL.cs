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


namespace DAL
{
    public class ReviewsDAL
    {
        DBConnection connection = new DBConnection();

        public bool InsertRestarantReview(int userID, int restaurantID, int ratingID, string reviewText, int reviewID)
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
                Trace.Listeners.Add(new TextWriterTraceListener("Error.log", "myListener"));
                Trace.TraceError(e.ToString());
                Trace.Flush();
                throw;
            }

                return inserted;
        }

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

                            reviews.Add(rdc);
                        }
                        
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

    }
}
