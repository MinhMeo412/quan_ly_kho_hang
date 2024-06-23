using WarehouseManager.UI;
using WarehouseManager.Data;

class Program
{
    public static WarehouseDatabase warehouse = new WarehouseDatabase("localhost", "root", "minhmeo1", "warehouse");

    public static void Main(String[] args)
    {
        UI.Start();
    }
}