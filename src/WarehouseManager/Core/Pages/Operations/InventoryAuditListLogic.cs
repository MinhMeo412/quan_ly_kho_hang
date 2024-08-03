using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
{
    public static class InventoryAuditListLogic
    {
        // Lấy dữ liệu về và đổi kiểu dữ liệu sang dạng DataTable
        public static DataTable GetData()
        {
            List<InventoryAudit> inventoryAudits = Program.Warehouse.InventoryAuditTable.InventoryAudits ?? new List<InventoryAudit>();
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            List<(int, string, string, string, DateTime?)> inventoryAuditMenuRows = new List<(int, string, string, string, DateTime?)>();

            foreach (InventoryAudit inventoryAudit in inventoryAudits)
            {
                User user = users.FirstOrDefault(u => u.UserID == inventoryAudit.UserID) ?? new User(0, "", "", "", "", "", 0);
                string userName = user.UserFullName;
                Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == inventoryAudit.WarehouseID) ?? new Warehouse(0, "", 0);
                string warehouseName = warehouse.WarehouseName;

                inventoryAuditMenuRows.Add((
                    inventoryAudit.InventoryAuditID,
                    warehouseName,
                    inventoryAudit.InventoryAuditStatus,
                    userName,
                    inventoryAudit.InventoryAuditTime));
            }

            return ConvertInventoryAuditMenuRowsToDataTable(inventoryAuditMenuRows);
        }

        // Đổi kiểu dữ liệu từ List<...> sang DataTable
        private static DataTable ConvertInventoryAuditMenuRowsToDataTable(List<(int, string, string, string, DateTime?)> inventoryAuditMenuRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Inventory Audit ID", typeof(int));
            dataTable.Columns.Add("Warehouse", typeof(string));
            dataTable.Columns.Add("Status");
            dataTable.Columns.Add("User Name", typeof(string));
            dataTable.Columns.Add("Date", typeof(DateTime));

            foreach (var inventoryAuditMenuRow in inventoryAuditMenuRows)
            {
                dataTable.Rows.Add(
                    inventoryAuditMenuRow.Item1,
                    inventoryAuditMenuRow.Item2,
                    inventoryAuditMenuRow.Item3,
                    inventoryAuditMenuRow.Item4,
                    inventoryAuditMenuRow.Item5);
            }

            return dataTable;
        }

        // Đổi kiểu dữ liệu từ DataTable sang List<...> (để thực hiện LINQ)
        private static List<(int, string, string, string, DateTime?)> ConvertDataTableToInventoryAuditMenuRows(DataTable dataTable)
        {
            List<(int, string, string, string, DateTime?)> inventoryAuditMenuRows = new List<(int, string, string, string, DateTime?)>();

            foreach (DataRow row in dataTable.Rows)
            {
                inventoryAuditMenuRows.Add(((int)row[0], (string)row[1], (string)row[2], (string)row[3], (DateTime?)row[4]));
            }
            return inventoryAuditMenuRows;
        }


        /// <summary>
        /// Sort theo từ mà người dùng nhập vào ô tìm kiếm
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="searchTerm"></param>
        /// <returns>DataTable</returns>
        public static DataTable SortInventoryAuditBySearchTerm(DataTable dataTable, string searchTerm)
        {
            return SortDataTable.BySearchTerm(dataTable, searchTerm);
        }


        /// <summary>
        /// sort theo cột mà người dùng bấm vào
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columnToSortBy"></param>
        /// <param name="sortColumnInDescendingOrder"></param>
        /// <returns>DataTable</returns>
        public static DataTable SortInventoryAuditListByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            return SortDataTable.ByColumn(dataTable, columnToSortBy, sortColumnInDescendingOrder);
        }

        //Delete
        public static DataTable DeleteInventoryAudit(DataTable dataTable, int inventoryAuditID)
        {

            Program.Warehouse.InventoryAuditTable.Delete(inventoryAuditID);

            List<(int, string, string, string, DateTime?)> inventoryAudits = ConvertDataTableToInventoryAuditMenuRows(dataTable);

            inventoryAudits.RemoveAll(s => s.Item1 == inventoryAuditID);

            return ConvertInventoryAuditMenuRowsToDataTable(inventoryAudits);
        }
    }
}