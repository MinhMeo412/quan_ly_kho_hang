using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core
{
    public static class UIComponentLogic
    {
        // Lấy về tên quyền người dùng
        public static string PermissionName()
        {
            int? permissionLevel = Program.Warehouse.PermissionLevel;
            string permissionName = "Unknown Permission";

            List<Permission>? permissions = Program.Warehouse.PermissionTable.Permissions;
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