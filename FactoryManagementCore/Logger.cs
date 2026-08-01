using FactoryManagementCore;
using Microsoft.Data.SqlClient;

public class Logger
{
    // ============================================================
    // Adds a new activity record into AuditLogs table.
    //
    // Parameters:
    // username    : User who performed the action
    // action      : Type of operation performed
    // module      : Application section where action occurred
    // description : Detailed information about the activity
    // status      : Result of operation (Success / Failed)
    //
    // Used for tracking important administrative activities.
    // ============================================================

    public static void AddLog(
        string username,
        string action,
        string module,
        string description,
        string status)
    {
        // Defensive logging: truncate fields to avoid DB truncation errors
        try
        {
            string Truncate(string? s, int max) => string.IsNullOrEmpty(s) ? string.Empty : (s.Length <= max ? s : s.Substring(0, max));

            const int MaxUser = 100;
            const int MaxAction = 100;
            const int MaxModule = 100;
            const int MaxDescription = 2000;
            const int MaxStatus = 200;

            var u = Truncate(username, MaxUser);
            var a = Truncate(action, MaxAction);
            var m = Truncate(module, MaxModule);
            var d = Truncate(description, MaxDescription);
            var s = Truncate(status, MaxStatus);

            using (SqlConnection con = new SqlConnection(DBHelper.ConnectionString))
            {
                con.Open();

                string query = @"
                INSERT INTO AuditLogs
                (
                    Username,
                    Action,
                    Module,
                    Description,
                    Status,
                    LogDate
                )
                VALUES
                (
                    @Username,
                    @Action,
                    @Module,
                    @Description,
                    @Status,
                    GETDATE()
                )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Use explicit parameter sizes to match truncated values
                    cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, MaxUser).Value = u;
                    cmd.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, MaxAction).Value = a;
                    cmd.Parameters.Add("@Module", System.Data.SqlDbType.NVarChar, MaxModule).Value = m;
                    cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, MaxDescription).Value = d;
                    cmd.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, MaxStatus).Value = s;

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch
        {
            // Swallow logging exceptions to avoid affecting main flow
        }
    }
}