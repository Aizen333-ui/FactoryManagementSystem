using FactoryManagementCore;
using Microsoft.Data.SqlClient;

public class Logger
{
    public static void AddLog(
        string username,
        string action,
        string module,
        string description,
        string status)
    {

        using (SqlConnection con = new SqlConnection(DBHelper.ConnectionString))
        {

            con.Open();

            string query =
            @"INSERT INTO AuditLogs
            (Username, Action, Module, Description, Status, LogDate)
            VALUES
            (@Username,@Action,@Module,@Description,@Status,GETDATE())";


            SqlCommand cmd =
            new SqlCommand(query, con);


            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@Module", module);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@LogDate", DateTime.Now);


            cmd.ExecuteNonQuery();
        }

    }
}