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

        public static void ExportToExcel(string selectedPath, DataTable table1, string sheetName1, DataTable table2, string sheetName2)
        {
            selectedPath = GetExcelFileName(selectedPath);
            Excel.Export(selectedPath, table1, sheetName1, table2, sheetName2);
        }

        public static string GetExcelFileName(string fileName)
        {
            if (!fileName.EndsWith(".xlsx"))
            {
                fileName = $"{fileName}.xlsx";
            }
            return fileName;
        }
    }
}