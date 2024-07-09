using WarehouseManager.Data.Entity;
using System.Data;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
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

        public static void ExportToExcel(DataTable dataTable, string selectedPath)
        {
            if (!selectedPath.Contains(".xlsx"))
            {
                selectedPath = $"{selectedPath}.xlsx";
            }
            Excel.Export(dataTable, selectedPath);
        }
    }
}