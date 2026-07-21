using Microsoft.Data.SqlClient;
using System.Data;
namespace FactoryManagementCore
{
    // Helper class for all database operations
    public static class DBHelper
    {
        // Database connection string
        static string conStr =
            "Data Source=localhost\\SQLEXPRESS;Initial Catalog=FactoryDB;Integrated Security=True;TrustServerCertificate=True";
        public static string ConnectionString => conStr;
        // Executes SELECT queries and returns result in DataTable
        public static DataTable ExecuteDataTable(string query, SqlParameter[] parameters)
        {
            // Create database connection
            using (SqlConnection con = new SqlConnection(conStr))
            {
                // Create SQL command
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add parameters if available
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    // Fill DataTable with query result
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Return result table
                    return dt;
                }
            }
        }

        // Executes INSERT, UPDATE, DELETE queries
        // Returns number of affected rows
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            // Create database connection
            using (SqlConnection con = new SqlConnection(conStr))
            {
                // Create SQL command
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add parameters if available
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    // Open database connection
                    con.Open();

                    // Execute query and return affected rows
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Executes queries that return a single value
        // Example: COUNT, MAX, SUM
        public static object ExecuteScalar(string query, SqlParameter[] parameters)
        {
            // Create database connection
            using (SqlConnection con = new SqlConnection(conStr))
            {
                // Create SQL command
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add parameters if available
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    // Open database connection
                    con.Open();

                    // Execute query and return single value
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}