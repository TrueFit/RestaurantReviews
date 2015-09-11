using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class Reviews
    {
        DBConnection connection = new DBConnection();

        public bool InsertRestarantReview()
        {
            bool inserted = false;
                using (SqlCommand cmd = new SqlCommand("InsertReview", connection.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@RestaurantID", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@RatingID", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@ReviewText", SqlDbType.NVarChar).Value = "this is from the DAL?";
                    cmd.Parameters.Add("@ReviewID", SqlDbType.Int).Value = 1;

                    return inserted;
                }
            
        }

    }
}
