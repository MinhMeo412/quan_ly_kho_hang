using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseListLogic
    {
        // Lấy dữ liệu về và đổi kiểu dữ liệu sang dạng DataTable
        public static DataTable GetData()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            List<WarehouseAddress> warehouseAddresses = Program.Warehouse.WarehouseAddressTable.WarehouseAddresses ?? new List<WarehouseAddress>();

            List<(int, string, string)> warehouseMenuRows = new List<(int, string, string)>();

            foreach (Warehouse warehouse in warehouses)
            {
                WarehouseAddress warehouseAddress = warehouseAddresses.FirstOrDefault(w => w.WarehouseAddressID == warehouse.WarehouseAddressID) ?? new WarehouseAddress(0, "", "", "", "", "");
                string warehouseAdressAddress = warehouseAddress.WarehouseAddressAddress;

                warehouseMenuRows.Add((
                    warehouse.WarehouseID,
                    warehouse.WarehouseName,
                    warehouseAdressAddress));
            }

            return ConvertWarehouseMenuRowsToDataTable(warehouseMenuRows);
        }

        // Đổi kiểu dữ liệu từ List<...> sang DataTable
        private static DataTable ConvertWarehouseMenuRowsToDataTable(List<(int, string, string)> warehouseMenuRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Warehouse ID", typeof(int));
            dataTable.Columns.Add("Warehouse Name", typeof(string));
            dataTable.Columns.Add("Warehouse Address", typeof(string));

            foreach (var warehouseMenuRow in warehouseMenuRows)
            {
                dataTable.Rows.Add(
                    warehouseMenuRow.Item1,
                    warehouseMenuRow.Item2,
                    warehouseMenuRow.Item3);
            }

            return dataTable;
        }

        // Đổi kiểu dữ liệu từ DataTable sang List<...> (để thực hiện LINQ)
        private static List<(int, string, string)> ConvertDataTableToWarehouseMenuRows(DataTable dataTable)
        {
            List<(int, string, string)> warehouseMenuRows = new List<(int, string, string)>();

            foreach (DataRow row in dataTable.Rows)
            {
                warehouseMenuRows.Add(((int)row[0], (string)row[1], (string)row[2]));
            }
            return warehouseMenuRows;
        }


        /// <summary>
        /// Sort theo từ mà người dùng nhập vào ô tìm kiếm
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="searchTerm"></param>
        /// <returns>DataTable</returns>
        public static DataTable SortWarehouseBySearchTerm(DataTable dataTable, string searchTerm)
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
        public static DataTable SortWarehouseListByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            return SortDataTable.ByColumn(dataTable, columnToSortBy, sortColumnInDescendingOrder);
        }

        //Update
        public static void UpdateWarehouse(int warehouseID, string warehouseName, int? warehouseAddressID)
        {

            // Workaround for a bug in terminal.gui that will crash the program if a row in a dropdown has a completely blank name.
            if (warehouseName == "")
            {
                warehouseName = " ";
            }
            Program.Warehouse.WarehouseTable.Update(warehouseID, warehouseName, warehouseAddressID);
        }

        //Delete
        public static DataTable DeleteWarehouse(DataTable dataTable, int warehouseID)
        {

            Program.Warehouse.WarehouseTable.Delete(warehouseID);

            List<(int, string, string)> warehouses = ConvertDataTableToWarehouseMenuRows(dataTable);

            warehouses.RemoveAll(s => s.Item1 == warehouseID);

            return ConvertWarehouseMenuRowsToDataTable(warehouses);
        }
    }
}