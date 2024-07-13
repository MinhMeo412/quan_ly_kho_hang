using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddAccountLogic
    {
        public static List<string> GetPermissionNames()
        {
            List<Permission> permissions = Program.Warehouse.PermissionTable.Permissions ?? new List<Permission>();
            List<string> permissionNames = permissions.Select(p => $"{p.PermissionName}").ToList();

            return permissionNames;
        }

        public static void Save(string userName, string userPassword, string userFullName, string? userEmail, string? userPhoneNumber, string permissionName)
        {
            AddAccount(userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionName);
        }

        private static void AddAccount(string userName, string userPassword, string userFullName, string? userEmail, string? userPhoneNumber, string permissionName)
        {
            int permissionLevel = GetPermissionLevel(permissionName);
            int userID = GetCurrentHighestUserID() + 1;
            Program.Warehouse.UserTable.Add(userID, userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionLevel);
        }

        private static int GetCurrentHighestUserID()
        {
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            int highestUserID = users.Max(u => u.UserID);
            return highestUserID;
        }

        private static int GetPermissionLevel(string permissionName)
        {
            List<Permission>? permissions = Program.Warehouse.PermissionTable.Permissions ?? new List<Permission>();
            Permission? permission = permissions.FirstOrDefault(c => c.PermissionName == permissionName);

            if (permission != null)
            {
                return permission.PermissionLevel;
            }
            return 4;
        }
    }
}