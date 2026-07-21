using FactoryManagementCore;

namespace FactoryManagementAdminTool
{
    public static class AdminChecker
    {
        public static bool AdminExists()
        {
            string query = "SELECT COUNT(*) FROM SystemAdmins";

            object result = DBHelper.ExecuteScalar(query, null);

            int count = Convert.ToInt32(result);

            return count > 0;
        }
    }
}
