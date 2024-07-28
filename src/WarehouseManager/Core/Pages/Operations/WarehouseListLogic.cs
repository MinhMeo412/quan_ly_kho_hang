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
            List<Warehouse> warehouses = GetWarehouses();
            List<WarehouseAddress> warehouseAddresses = GetWarehouseAddresses();

            List<(int, string, string, string, string, string, string)> warehouseMenuRows = new List<(int, string, string, string, string, string, string)>();

            foreach (Warehouse warehouse in warehouses)
            {
                WarehouseAddress warehouseAddress = GetWarehouseAddress(warehouse.WarehouseAddressID ?? 0, warehouseAddresses);

                warehouseMenuRows.Add((
                    warehouse.WarehouseID,
                    warehouse.WarehouseName,
                    warehouseAddress.WarehouseAddressAddress,
                    $"{warehouseAddress.WarehouseAddressDistrict}",
                    $"{warehouseAddress.WarehouseAddressPostalCode}",
                    $"{warehouseAddress.WarehouseAddressCity}",
                    $"{warehouseAddress.WarehouseAddressCountry}"
                ));
            }

            return ConvertWarehouseMenuRowsToDataTable(warehouseMenuRows);
        }

        // Đổi kiểu dữ liệu từ List<...> sang DataTable
        private static DataTable ConvertWarehouseMenuRowsToDataTable(List<(int, string, string, string, string, string, string)> warehouseMenuRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Warehouse ID", typeof(int));
            dataTable.Columns.Add("Warehouse Name", typeof(string));
            dataTable.Columns.Add("Warehouse Address", typeof(string));
            dataTable.Columns.Add("District", typeof(string));
            dataTable.Columns.Add("Postal Code", typeof(string));
            dataTable.Columns.Add("City", typeof(string));
            dataTable.Columns.Add("Country", typeof(string));

            foreach (var warehouseMenuRow in warehouseMenuRows)
            {
                dataTable.Rows.Add(
                    warehouseMenuRow.Item1,
                    warehouseMenuRow.Item2,
                    warehouseMenuRow.Item3,
                    warehouseMenuRow.Item4,
                    warehouseMenuRow.Item5,
                    warehouseMenuRow.Item6,
                    warehouseMenuRow.Item7);
            }

            return dataTable;
        }

        // Đổi kiểu dữ liệu từ DataTable sang List<...> (để thực hiện LINQ)
        private static List<(int, string, string, string, string, string, string)> ConvertDataTableToWarehouseMenuRows(DataTable dataTable)
        {
            List<(int, string, string, string, string, string, string)> warehouseMenuRows = new List<(int, string, string, string, string, string, string)>();

            foreach (DataRow row in dataTable.Rows)
            {
                warehouseMenuRows.Add(((int)row[0], (string)row[1], (string)row[2], (string)row[3], (string)row[4], (string)row[5], (string)row[6]));
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
        public static void Update(int warehouseID, string warehouseName, string warehouseAddress, string warehosuseDistrict, string warehousePostalCode, string warehouseCity, string warehouseCountry)
        {

            // Workaround for a bug in terminal.gui that will crash the program if a row in a dropdown has a completely blank name.
            if (warehouseName == "")
            {
                warehouseName = " ";
            }

            int warehouseAddressID = GetWarehouseAddressID(warehouseID);

            UpdateWarehouse(warehouseID, warehouseName, warehouseAddressID);
            UpdateWarehouseAddresses(warehouseAddressID, warehouseAddress, warehosuseDistrict, warehousePostalCode, warehouseCity, warehouseCountry);


        }

        //Delete
        public static DataTable DeleteWarehouse(DataTable dataTable, int warehouseID)
        {

            Program.Warehouse.WarehouseTable.Delete(warehouseID);

            List<(int, string, string, string, string, string, string)> warehouses = ConvertDataTableToWarehouseMenuRows(dataTable);

            warehouses.RemoveAll(s => s.Item1 == warehouseID);

            return ConvertWarehouseMenuRowsToDataTable(warehouses);
        }

        private static List<Warehouse> GetWarehouses()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouses();
        }

        private static List<WarehouseAddress> GetWarehouseAddresses()
        {
            List<WarehouseAddress> warehouseAddresses = Program.Warehouse.WarehouseAddressTable.WarehouseAddresses ?? new List<WarehouseAddress>();
            return warehouseAddresses;
        }

        private static WarehouseAddress GetWarehouseAddress(int warehouseAddressID, List<WarehouseAddress> warehouseAddresses)
        {
            WarehouseAddress warehouseAddress = warehouseAddresses.
                FirstOrDefault(w => w.WarehouseAddressID == warehouseAddressID) ?? new WarehouseAddress(0, "", "", "", "", "");

            return warehouseAddress;
        }

        private static void UpdateWarehouse(int warehouseID, string warehouseName, int warehouseAddressID)
        {
            Program.Warehouse.WarehouseTable.Update(warehouseID, warehouseName, warehouseAddressID);
        }

        private static void UpdateWarehouseAddresses(int warehouseAddressID, string warehouseAddress, string warehosuseDistrict, string warehousePostalCode, string warehouseCity, string warehouseCountry)
        {
            Program.Warehouse.WarehouseAddressTable.Update(warehouseAddressID, warehouseAddress, warehosuseDistrict, warehousePostalCode, warehouseCity, warehouseCountry);
        }

        private static int GetWarehouseAddressID(int warehouseID)
        {
            List<Warehouse> warehouses = GetWarehouses();
            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == warehouseID) ?? new Warehouse(0, "", 0);
            return warehouse.WarehouseAddressID ?? 0;
        }
    }
}