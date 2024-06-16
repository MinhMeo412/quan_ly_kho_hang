using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class UserTable
    {
        public List<User>? Users { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawUsers = Procedure.ExecuteReader(connectionString, "read_user", inParameters);

            List<User> users = new List<User>();
            foreach (List<object?> rawUser in rawUsers)
            {
                User user = new User(
                    (int)(rawUser[0] ?? 0),
                    (string)(rawUser[1] ?? ""),
                    (string)(rawUser[2] ?? ""),
                    (string?)rawUser[3],
                    (string?)rawUser[4],
                    (string?)rawUser[5],
                    (int)(rawUser[6] ?? 0)
                );
                users.Add(user);
            }

            this.Users = users;
        }

        public void Add(string connectionString, string token, int userID, string userName, string userPassword, string? userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_user_id", userID},
                {"input_user_name", userName},
                {"input_user_password", userPassword},
                {"input_user_full_name", userFullName},
                {"input_user_email", userEmail},
                {"input_user_phone_number", userPhoneNumber},
                {"input_permission_level", permissionLevel}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_user", inParameters);

            User user = new User(userID, userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionLevel);

            this.Users ??= new List<User>();
            this.Users.Add(user);
        }

        public void Update(string connectionString, string token, int userID, string userName, string userPassword, string? userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_user_id", userID},
                {"input_user_name", userName},
                {"input_user_password", userPassword},
                {"input_user_full_name", userFullName},
                {"input_user_email", userEmail},
                {"input_user_phone_number", userPhoneNumber},
                {"input_permission_level", permissionLevel}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_user", inParameters);

            var user = this.Users?.FirstOrDefault(u => u.UserID == userID);
            if (user != null)
            {
                user.UserName = userName;
                user.UserPassword = userPassword;
                user.UserFullName = userFullName;
                user.UserEmail = userEmail;
                user.UserPhoneNumber = userPhoneNumber;
                user.PermissionLevel = permissionLevel;
            }
        }

        public void Delete(string connectionString, string token, int userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_user_id", userID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_user", inParameters);

            var user = this.Users?.FirstOrDefault(u => u.UserID == userID);
            if (user != null)
            {
                this.Users ??= new List<User>();
                this.Users.Remove(user);
            }
        }
    }
}
