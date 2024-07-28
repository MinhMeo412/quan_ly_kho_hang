using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddWarehouseLogic
    {
        public static void Save(string warehouseName, string warehouseAddress, string warehouseDistrict, string warehousePostalCode, string warehouseCity, string warehouseCountry)
        {
            // Workaround for a bug in terminal.gui that will crash the program if a row in a dropdown has a completely blank name.
            if (warehouseName == "")
            {
                warehouseName = " ";
            }

            int warehouseAddressID = GetCurrentHighestWarehouseAddressID() + 1;
            AddWarehouseAddress(warehouseAddressID, warehouseAddress, warehouseDistrict, warehousePostalCode, warehouseCity, warehouseCountry);
            AddWarehouse(warehouseName, warehouseAddressID);
        }

        private static void AddWarehouse(string warehouseName, int warehouseAddressID)
        {
            int warehouseID = GetCurrentHighestWarehouseID() + 1;
            Program.Warehouse.WarehouseTable.Add(warehouseID, warehouseName, warehouseAddressID);
        }

        private static void AddWarehouseAddress(int warehouseAddressID, string warehouseAddress, string warehouseDistrict, string warehousePostalCode, string warehouseCity, string warehouseCountry)
        {
            Program.Warehouse.WarehouseAddressTable.Add(warehouseAddressID, warehouseAddress, warehouseDistrict, warehousePostalCode, warehouseCity, warehouseCountry);
        }

        private static int GetCurrentHighestWarehouseAddressID()
        {
            return GetWarehouseAddresses().Max(wa => wa.WarehouseAddressID);
        }

        private static int GetCurrentHighestWarehouseID()
        {
            return GetWarehouses().Max(w => w.WarehouseID);
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
    }
}