using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManager.Data.Entity;
using System.Data.SqlClient;
using System.Data;

namespace WarehouseManager.Data.Table
{
    class SupplierTable
    {
        public List<Supplier>? Suppliers { get; private set; }
        
        private void Load(string connectionString,string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token",token }
            };

            List<List<object>> rawPermissions = Procedure.ExecuteReader(connectionString, "read_permission", inParameters);

            List<Permission> permissions = new List<Permission>();
            foreach(List<object> rawPermission in rawPermissions)
            {
                Permission permission=new Permission((int)rawPermission[0], (string)rawPermission[1], (string)rawPermission[2]);
                permissions.Add(permission);
            }
            this.Add(permission);
        }

        public void add(string connectionString, string token, int permissionLevel, string permissionName, string permissionDescription)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"input_permission_level", permissionLevel},
                {"input_permission_name", permissionName},
                {"input_permission_description", permissionDescription}
            };

            Procedure.ExecuteNonQuery(connectionString, "create_supplier", inParameters);

            Permission permission = new Permission(permissionLevel, permissionName, permissionDescription);


            this.Permissions ??= new List<Permission>();
            this.Permissions.Add(permission);
        }
        
        public void Update(string connectionString, string token, int permissionLevel, string permissionName, string permissionDescription)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"input_permission_level", permissionLevel},
                {"input_permission_name", permissionName},
                {"input_permission_description", permissionDescription}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_supplier", inParameters);

            var permission=this.Permissions?.FirstOrDefault(p => p.PermissionLevel == permissionLevel);
        }
    }
}