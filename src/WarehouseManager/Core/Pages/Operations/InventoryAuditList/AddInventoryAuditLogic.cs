using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddInventoryAuditLogic
    {
        public static DataTable GetDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Product");
            dataTable.Columns.Add("Stock Amount");
            dataTable.Columns.Add("Actual Amount");
            dataTable.Columns.Add("Difference");

            for (int i = 0; i < 100; i++)
            {
                dataTable.Rows.Add("", "", "", "", "");
            }


            return dataTable;
        }
    }
}
