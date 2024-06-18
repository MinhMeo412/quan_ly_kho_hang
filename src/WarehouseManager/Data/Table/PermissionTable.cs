using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class PermissionTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<Permission>? Permissions { get; private set; }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawPermissions = Procedure.ExecuteReader(this.ConnectionString, "read_permission", inParameters);

            List<Permission> permissions = new List<Permission>();
            foreach (List<object?> rawPermission in rawPermissions)
            {
                Permission permission = new Permission((int)(rawPermission[0] ?? 0), (string)(rawPermission[1] ?? ""), (string)(rawPermission[2] ?? ""));
                permissions.Add(permission);
            }

            this.Permissions = permissions;
        }

        public void Add(int permissionLevel, string permissionName, string permissionDescription)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_permission_level", permissionLevel},
                {"input_permission_name", permissionName},
                {"input_permission_description", permissionDescription}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_permission", inParameters);
        }

        public void Update(int permissionLevel, string permissionName, string permissionDescription)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_permission_level", permissionLevel},
                {"input_permission_name", permissionName},
                {"input_permission_description", permissionDescription}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_permission", inParameters);
        }

        public void Delete(int permissionLevel)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_permission_level", permissionLevel}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_permission", inParameters);
        }
    }
}