using WarehouseManager.Core;
using WarehouseManager.Data.Utility;

class Program
{
    public static void Main(String[] args)
    {
        string connectionString = "server=localhost; user=root; password=7777; database=warehouse";
        Dictionary<string, object> inParameters = new Dictionary<string, object> {
            {"input_shit",3}
        };
        List<string> outParameters = new List<string> {"output_shit"};
        Dictionary<string, object> output = Procedure.ExecuteNonQuery(connectionString, "testing_shit", inParameters, outParameters);
        Console.WriteLine(output["output_shit"]);
    }
}