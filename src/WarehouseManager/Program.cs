using WarehouseManager.UI;
using WarehouseManager.Data;

class Program
{
    public static WarehouseDatabase warehouse = new WarehouseDatabase("localhost", "root", "7777", "warehouse");

    public static void Main(String[] args)
    {
        UI.Start();
    }
}