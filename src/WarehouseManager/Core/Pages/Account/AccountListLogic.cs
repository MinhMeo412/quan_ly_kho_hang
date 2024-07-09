using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
{
    public static class UserListLogic
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
            dataTable.Columns.Add("User Name", typeof(string));
            dataTable.Columns.Add("User Password", typeof(string));
            dataTable.Columns.Add("Full Name", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Phone Number", typeof(string));
            dataTable.Columns.Add("Permission Level", typeof(int));

            users ??= new List<User>();
            foreach (User user in users)
            {
                dataTable.Rows.Add(user.UserID, user.UserName, user.UserPassword, user.UserFullName, user.UserEmail, user.UserPhoneNumber, user.PermissionLevel);
            }
            return dataTable;
        }
        private static List<User> ConvertDataTableToUserList(DataTable dataTable)
        {
            List<User> users = new List<User>();
            foreach (DataRow row in dataTable.Rows)
            {
                User user = new User((int)row[0], (string)row[1], (string)row[2], (string)row[3], (string?)row[4], (string?)row[5], (int)row[6]);
                users.Add(user);
            }
            return users;
        }
        public static DataTable SortUserBySearchTerm(DataTable dataTable, string searchTerm)
        {
            List<User> users = ConvertDataTableToUserList(dataTable);
            List<(User, double)> userSimilarities = new List<(User, double)>();
            foreach (User user in users)
            {
                double maxSimilarity = Misc.MaxDouble(
                    Misc.JaccardSimilarity($"{user.UserID}", searchTerm),
                    Misc.JaccardSimilarity(user.UserName, searchTerm),
                    Misc.JaccardSimilarity(user.UserPassword, searchTerm),
                    Misc.JaccardSimilarity(user.UserFullName, searchTerm),
                    Misc.JaccardSimilarity(user.UserEmail ?? "", searchTerm),
                    Misc.JaccardSimilarity(user.UserPhoneNumber ?? "", searchTerm),
                    Misc.JaccardSimilarity($"{user.PermissionLevel}", searchTerm)
                );
                userSimilarities.Add((user, maxSimilarity));
            }
            List<User> sortedUsers = userSimilarities
              .OrderByDescending(cs => cs.Item2)
              .Select(cs => cs.Item1)
              .ToList();
            return ConvertUserListToDataTable(sortedUsers);
        }
        public static DataTable SortUserByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            List<User> users = ConvertDataTableToUserList(dataTable);

            switch (columnToSortBy)
            {
                case 0:
                    users = users.OrderBy(c => c.UserID).ToList();
                    break;
                case 1:
                    users = users.OrderBy(c => c.UserName).ToList();
                    break;
                case 2:
                    users = users.OrderBy(c => c.UserPassword).ToList();
                    break;
                case 3:
                    users = users.OrderBy(c => c.UserFullName).ToList();
                    break;
                case 4:
                    users = users.OrderBy(c => c.UserEmail).ToList();
                    break;
                case 5:
                    users = users.OrderBy(c => c.UserPhoneNumber).ToList();
                    break;
                case 6:
                    users = users.OrderBy(c => c.PermissionLevel).ToList();
                    break;
                default:
                    break;
            }
            if (sortColumnInDescendingOrder)
            {
                users.Reverse();
            }
            DataTable sortedDataTable = ConvertUserListToDataTable(users);
            sortedDataTable.Columns[columnToSortBy].ColumnName = Misc.ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);
            return sortedDataTable;
        }


        public static void UpdateUser(int userID, string userName, string userPassword, string userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
        {
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