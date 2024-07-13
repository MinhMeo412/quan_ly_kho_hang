using WarehouseManager.UI;
using WarehouseManager.Data;

class Program
{
    public static WarehouseDatabase Warehouse = new WarehouseDatabase("localhost", "warehouse_app_user", "1234", "warehouse");

    public static void Main()
    {
        // dotnet publish -c Release -r win-x64 --self-contained
        // dotnet publish -c Release -r linux-x64 --self-contained
        // dotnet publish -c Release -r osx-x64 --self-contained

        UI.Start();
    }
}