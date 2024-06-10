using WarehouseManager.Data;

class Program
{
    public static void Main(String[] args)
    {
        string username = "patrickmoore";
        string user_password = "L)7z7Op_XV";

        string server = "localhost";
        string user = "root";
        string password = "7777";
        string database = "warehouse";
        WarehouseDatabase warehouse = new WarehouseDatabase(server, user, password, database);

        Console.WriteLine(warehouse.Login(username,user_password));
    }
}