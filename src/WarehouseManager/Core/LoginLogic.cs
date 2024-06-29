
namespace WarehouseManager.Core
{
    public static class LoginLogic
    {
        public static bool Check(string username, string password)
        {
            return Program.warehouse.Login(username, password);
        }
    }
}