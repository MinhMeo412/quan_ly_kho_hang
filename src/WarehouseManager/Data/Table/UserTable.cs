using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class UserTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<User>? _users;
        public List<User>? Users
        {
            get
            {
                this.Load();
                return this._users;
            }
            private set
            {
                this._users = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawUsers = Procedure.ExecuteReader(this.ConnectionString, "read_user", inParameters);

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

        public void Add(int userID, string userName, string userPassword, string? userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_user_id", userID},
                {"input_user_name", userName},
                {"input_user_password", userPassword},
                {"input_user_full_name", userFullName},
                {"input_user_email", userEmail},
                {"input_user_phone_number", userPhoneNumber},
                {"input_permission_level", permissionLevel}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_user", inParameters);
        }

        public void Update(int userID, string userName, string userPassword, string? userFullName, string? userEmail, string? userPhoneNumber, int permissionLevel)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_user_id", userID},
                {"input_user_name", userName},
                {"input_user_password", userPassword},
                {"input_user_full_name", userFullName},
                {"input_user_email", userEmail},
                {"input_user_phone_number", userPhoneNumber},
                {"input_permission_level", permissionLevel}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_user", inParameters);
        }

        public void Delete(int userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_user_id", userID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_user", inParameters);
        }
    }
}
