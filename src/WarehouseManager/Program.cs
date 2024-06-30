using WarehouseManager.UI;
using WarehouseManager.Data;

class Program
{
    public static WarehouseDatabase Warehouse = new WarehouseDatabase("localhost", "warehouse_app_user", "1234", "warehouse");

    public static void Main()
    {
        UI.Start();
    }
}