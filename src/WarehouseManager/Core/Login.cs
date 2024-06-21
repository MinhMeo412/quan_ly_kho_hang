
namespace WarehouseManager.Core
{
    static class Login
    {
        public static bool Check(string username, string password)
        {
            return Program.warehouse.Login(username, password);
        }
    }
}