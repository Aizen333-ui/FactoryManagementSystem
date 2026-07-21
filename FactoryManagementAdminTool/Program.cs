namespace FactoryManagementAdminTool
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

           

            if (AdminChecker.AdminExists())
            {
               
                Application.Run(new AdminLogin());
            }
            else
            {
                
                Application.Run(new FirstAdminSetup());
            }
        }
    }
}