using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Pages;

namespace WarehouseManager.Core.Pages
{
    public static class AddAccountLogic
    {
        public static void AddAccount(int userID, string userName, string userPassword, string userFullName, string? userEmail, string? userPhoneNumber, string permissionName)
        {

            List<Permission>? permissions = Program.Warehouse.PermissionTable.Permissions ?? new List<Permission>();
            Permission? permission = permissions.FirstOrDefault(c => c.PermissionName == permissionName);

            int permissionLevel = 0;

            if (permission != null)
            {
                permissionLevel = permission.PermissionLevel;
            }
            Program.Warehouse.UserTable.Add(userID, userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionLevel);
        }

        public static void Save(string userName, string userPassword, string userFullName, string? userEmail, string? userPhoneNumber, string permissionName)
        {
            int userID = GetCurrentHighestUserID() + 1;
            AddAccount(userID, userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionName);
        }

        private static int GetCurrentHighestUserID()
        {
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            int highestUserID = users.Max(u => u.UserID);
            return highestUserID;
        }
    }
}