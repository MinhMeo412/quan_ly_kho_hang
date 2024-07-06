namespace WarehouseManager.Core.Pages
{
    public static class LoginLogic
    {
        public static bool Check(string username, string password)
        {
            return Program.Warehouse.Login(username, password);
        }
    }
}