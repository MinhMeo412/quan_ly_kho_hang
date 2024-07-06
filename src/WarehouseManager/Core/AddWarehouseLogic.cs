using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core
{
    public static class AddWarehouseLogic
    {
        public static bool AddWarehouse(string warehouseName)
        {
            // Workaround for a bug in terminal.gui that will crash the program if a row in a dropdown has a completely blank name.
            if (warehouseName == "")
            {
                warehouseName = " ";
            }

            List<Warehouse>? warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            if (warehouses.Count >= 10)
            {
                Console.WriteLine("Cannot add more than 10 warehouses.");
                return false;
            }
            else
            {
                int highestWarehouesID = warehouses.Max(w => w.WarehouseID);
                int warehouseAddressID = 1;
                Program.Warehouse.WarehouseTable.Add(highestWarehouesID + 1, warehouseName, warehouseAddressID);
                return true;
            }
        }
    }
}