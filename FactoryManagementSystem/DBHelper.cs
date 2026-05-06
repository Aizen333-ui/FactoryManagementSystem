using Microsoft.Data.SqlClient;
using System.Data;

class DBHelper
{
    static string conStr = "Data Source=TALHA-SOHAIL\\SQLEXPRESS;Initial Catalog=FactoryDB;Integrated Security=True;TrustServerCertificate=True";

    public static DataTable ExecuteDataTable(string query, SqlParameter[] parameters)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    public static int ExecuteNonQuery(string query, SqlParameter[] parameters)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }

    // ✅ ADD THIS METHOD (fix for your error)
    public static object ExecuteScalar(string query, SqlParameter[] parameters)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}