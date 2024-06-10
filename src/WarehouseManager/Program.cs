using WarehouseManager.Data;
using WarehouseManager.Data.Table;

class Program
{
    public static void Main(String[] args)
    {
        string username = "garciakaren";
        string user_password = "8)3EetoYf!";

        string server = "localhost";
        string user = "root";
        string password = "7777";
        string database = "warehouse";
        WarehouseDatabase warehouse = new WarehouseDatabase(server, user, password, database);

        Console.WriteLine(warehouse.Login(username, user_password));

        PermissionTable permissionTable = new PermissionTable();

        string token = "d2b147e0-2729-11ef-a4ec-56e8b7964de4";
        string connectionString = $"server={server}; user={user}; password={password}; database={database}";
        permissionTable.Load(connectionString, token);
        
        // permissionTable.Add(connectionString, token, -2147483648, "fieuafha3d", "poopy");
        // permissionTable.Update(connectionString, token, -2147483648, "this is updated", "poopy");
        // permissionTable.Delete(connectionString, token, 3);


    }
}