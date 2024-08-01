using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddInventoryAuditLogic
    {
        public static List<string> GetWarehouseNames()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouseList();
        }

        public static Dictionary<int, string> GetVariantList()
        {
            // TODO
            return new Dictionary<int, string>();
        }

        public static int GetVariantID(string variantName, Dictionary<int, string> variantList)
        {
            // TODO
            return 0;
        }

        public static int GetVariantIndex(string variantID, Dictionary<int, string> variantList)
        {
            // TODO
            return 0;
        }

        public static DataTable AddVariant(string variantID, string quantity, DataTable dataTable)
        {
            // TODO
            return new DataTable();
        }

        public static DataTable Search(DataTable dataTable, string searchTerm)
        {
            // TODO
            return new DataTable();
        }

        public static DataTable GetDataTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Product");
            dataTable.Columns.Add("Stock Amount");
            dataTable.Columns.Add("Actual Amount");
            dataTable.Columns.Add("Difference");

            return dataTable;
        }

        public static DataTable DeleteVariant(string variantID, DataTable dataTable)
        {
            // TODO
            return new DataTable();
        }

        public static DataTable GetAllStock(DataTable dataTable)
        {
            // TODO
            return new DataTable();
        }

        public static void Save(string warehouseName, string description, DataTable dataTable)
        {
            // TODO

        }
    }
}
