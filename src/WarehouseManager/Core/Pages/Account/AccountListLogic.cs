using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
{
    public static class AccountListLogic
    {
        public static DataTable GetData()
        {
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            return ConvertUserListToDataTable(users);
        }

        private static DataTable ConvertUserListToDataTable(List<User> users)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("User ID", typeof(int));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("Full Name", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Phone Number", typeof(string));
            dataTable.Columns.Add("Permission Level", typeof(string));

            List<Permission> permissions = Program.Warehouse.PermissionTable.Permissions ?? new List<Permission>();

            foreach (User user in users)
            {
                string permissionName = permissions.FirstOrDefault(p => p.PermissionLevel == user.PermissionLevel)?.PermissionName ?? "Unknown Permission";

                dataTable.Rows.Add(user.UserID, user.UserName, user.UserFullName, user.UserEmail, user.UserPhoneNumber, permissionName);
            }

            return dataTable;
        }

        private static List<User> ConvertDataTableToUserList(DataTable dataTable)
        {
            List<User> users = new List<User>();
            List<Permission> permissions = Program.Warehouse.PermissionTable.Permissions ?? new List<Permission>();

            foreach (DataRow row in dataTable.Rows)
            {
                Permission permission = permissions.FirstOrDefault(p => p.PermissionName == (string)row[5]) ?? new Permission(4, "", "");
                int permissionLevel = permission.PermissionLevel;

                User user = new User((int)row[0], (string)row[1], "", (string)row[2], (string?)row[3], (string?)row[4], permissionLevel);
                users.Add(user);
            }
            return users;
        }

        public static DataTable SortUserBySearchTerm(DataTable dataTable, string searchTerm)
        {
            return SortDataTable.BySearchTerm(dataTable, searchTerm);
        }

        public static DataTable SortUserByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            return SortDataTable.ByColumn(dataTable, columnToSortBy, sortColumnInDescendingOrder);
        }


        public static void UpdateUser(int userID, string userName, string userPassword, string userFullName, string? userEmail, string? userPhoneNumber, string permissionName)
        {
            List<Permission> permissions = Program.Warehouse.PermissionTable.Permissions ?? new List<Permission>();
            Permission permission = permissions.FirstOrDefault(p => p.PermissionName == permissionName) ?? new Permission(4, "", "");
            int permissionLevel = permission.PermissionLevel;

            Program.Warehouse.UserTable.Update(userID, userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionLevel);
        }

        public static DataTable DeleteUser(DataTable dataTable, int UserID)
        {
            Program.Warehouse.UserTable.Delete(UserID);
            List<User> users = ConvertDataTableToUserList(dataTable);
            users.RemoveAll(c => c.UserID == UserID);

            return ConvertUserListToDataTable(users);
        }
    }
}