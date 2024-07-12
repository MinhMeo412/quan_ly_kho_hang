namespace WarehouseManager.Core.Pages
{
    public static class ChangePasswordLogic
    {
        public static string GetUsername()
        {
            return $"{Program.Warehouse.Username}";
        }

        public static bool ChangePassword(string oldPassword, string newPassword)
        {
            return Program.Warehouse.ChangePassword(oldPassword, newPassword);
        }
    }
}

