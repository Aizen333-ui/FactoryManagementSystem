using FactoryManagementCore;

namespace FactoryManagementAdminTool
{
    public static class AdminChecker
    {
        //Checks for any existing admin account stored in the database. Returns true if at least one admin account exists, false otherwise and opens the FirstAdminSetupForm.
        public static bool AdminExists()
        {
            string query = "SELECT COUNT(*) FROM SystemAdmins";

            object result = DBHelper.ExecuteScalar(query, null);

            int count = Convert.ToInt32(result);

            return count > 0;
        }
    }
}
