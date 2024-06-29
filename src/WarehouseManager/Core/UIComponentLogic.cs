using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core
{
    public static class UIComponentLogic
    {
        public static string PermissionName()
        {
            int? permissionLevel = Program.warehouse.PermissionLevel;
            string permissionName = "Unknown Permission";
            if (permissionLevel == null)
            {
                return permissionName;
            }

            List<Permission>? permissions = Program.warehouse.PermissionTable.Permissions;
            if (permissions != null)
            {
                foreach (Permission permission in permissions)
                {
                    if (permissionLevel == permission.PermissionLevel)
                    {
                        permissionName = $"{permission.PermissionName}";
                    }
                }
            }

            return permissionName;
        }
    }
}