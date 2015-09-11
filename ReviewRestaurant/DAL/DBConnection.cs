using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DBConnection
    {
        private static string connectionString = "Data Source=MARTHA-PC\\SQLEXPRESS;Initial Catalog=ReviewDB;Integrated Security=True";

        public SqlConnection connection = new SqlConnection(connectionString);

        public SqlConnection GetConnection()
        {
            if(connection.State==ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        public int ExecNonQuery(SqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            int rows = cmd.ExecuteNonQuery();
            connection.Close();
            return rows;
        }

        public object ExecScalar(SqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            object o = cmd.ExecuteScalar();
            connection.Close();
            return o;
        }

        public DataTable SelectData(SqlCommand cmd)
        {
            cmd.Connection = GetConnection();
            DataTable dt = new DataTable();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            connection.Close();
            return dt;

        }

    }
}
