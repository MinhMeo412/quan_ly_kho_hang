using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class UserTable
    {
        public List<User>? Users { get; private set; }

        private void Load(string connectionString,string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>{
                {"input_token", token}
            };

            List<List<object>> rawUsers = Procedure.ExecuteReader(connectionString, "read_user", inParameters);

            List<User> users = new List<User>();
            foreach (List<object> rawUser in rawUser)
            {
                User user = new User((int)userID[0], (string)userName[1], (string)userPassword[2], (string)userFullName[3], (string)userEmail[4],(string)userPhoneNumber[5], (string)permissionLevel[6]);
                users.Add(user);
            }

            this.Users = users;
        }

        private void Add(string connectionString, string token, int userID, string userName, string userPassword, string? userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>{
                {"input_token", token},
                {"new_user_name", userName},
                {"new_user_password", userPassword},
                {"new_user_full_name", userFullName},
                {"new_user_email", userEmail},
                {"new_user_phone_number", userPhoneNumber},
                {"new_permission_level", permissionLevel}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_user", inParameters);

            User user = new User(userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionLevel);

            this.Users ??= new List<User>();
            this.Users.Add(user);
        }

        public void Update(string connectionString, string token, int userID, string userName, string userPassword, string? userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
            {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
               {"input_token", token},
                {"new_user_name", userName},
                {"new_user_password", userPassword},
                {"new_user_full_name", userFullName},
                {"new_user_email", userEmail},
                {"new_user_phone_number", userPhoneNumber},
                {"new_permission_level", permissionLevel}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_user", inParameters);

            var user = this.Users?.FirstOrDefault(u => u.UserID == userID);
            if (user != null)
            {
                user.UserName = userName;
                user.UserPassword = userPassword;
                user.UserFullName = userName;
                user.PermissionDescription = permissionDescription;
                user.UserEmail = userEmail;
                user.UserPhoneNumber = userPhoneNumber;
                user.PermissionLevel = permissionLevel;
            }
        }
    }
}

