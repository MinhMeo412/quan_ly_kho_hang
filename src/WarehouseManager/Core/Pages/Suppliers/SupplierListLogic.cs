using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;
using Org.BouncyCastle.Asn1.Icao;

namespace WarehouseManager.Core.Pages
{
    public static class SupplierListLogic
    {
        public static DataTable GetData()
        {
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            return ConvertSupplierListToDataTable(suppliers);
        }

        private static DataTable ConvertSupplierListToDataTable(List<Supplier> suppliers)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Supplier ID", typeof(int));
            dataTable.Columns.Add("Supplier Name", typeof(string));
            dataTable.Columns.Add("Supplier Description", typeof(string));
            dataTable.Columns.Add("Supplier Email", typeof(int));
            dataTable.Columns.Add("Supplier Phone Number", typeof(string));
            dataTable.Columns.Add("Supplier Website", typeof(string));

            suppliers ??= new List<Supplier>();
            foreach (Supplier supplier in suppliers)
            {
                dataTable.Rows.Add(supplier.SupplierID, supplier.SupplierName, supplier.SupplierDescription, supplier.SupplierAddress, supplier.SupplierEmail, supplier.SupplierPhoneNumber, supplier.SupplierWebsite);

            }
            return dataTable;
        }
        private static List<Supplier> ConvertDataTableToSupplierList(DataTable dataTable)
        {
            List<Supplier> suppliers = new List<Supplier>();
            foreach (DataRow row in dataTable.Rows)
            {
                Supplier supplier = new Supplier((int)row[0], (string)row[1], (string?)row[2], (string?)row[3], (string?)row[4], (string?)row[5], (string?)row[6]);
                suppliers.Add(supplier);
            }
            return suppliers;
        }

        public static DataTable SortSupplierBySearchTerm(DataTable dataTable, string searchTerm)
        {
            List<Supplier> suppliers = ConvertDataTableToSupplierList(dataTable);
            List<(Supplier, double)> supplierSimilarities = new List<(Supplier, double)>();

            foreach (Supplier supplier in suppliers)
            {
                double maxSimilarity = Misc.MaxDouble(
                 Misc.JaccardSimilarity($"{supplier.SupplierID}", searchTerm),
                 Misc.JaccardSimilarity(supplier.SupplierName, searchTerm),
                 Misc.JaccardSimilarity(supplier.SupplierDescription ?? "", searchTerm),
                 Misc.JaccardSimilarity(supplier.SupplierAddress ?? "", searchTerm),
                 Misc.JaccardSimilarity(supplier.SupplierEmail ?? "", searchTerm),
                 Misc.JaccardSimilarity(supplier.SupplierPhoneNumber ?? "", searchTerm),
                 Misc.JaccardSimilarity(supplier.SupplierWebsite ?? "", searchTerm)
                );

                supplierSimilarities.Add((supplier, maxSimilarity));
            }

            List<Supplier> sortedSuplier = supplierSimilarities
            .OrderByDescending(cs => cs.Item2)
            .Select(cs => cs.Item1)
            .ToList();

            return ConvertSupplierListToDataTable(sortedSuplier);
        }

        public static DataTable SortSupplierByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            List<Supplier> suppliers = ConvertDataTableToSupplierList(dataTable);

            switch (columnToSortBy)
            {
                case 0:
                    suppliers = suppliers.OrderBy(c => c.SupplierID).ToList();
                    break;
                case 1:
                    suppliers = suppliers.OrderBy(c => c.SupplierName).ToList();
                    break;
                case 2:
                    suppliers = suppliers.OrderBy(c => c.SupplierDescription).ToList();
                    break;
                case 3:
                    suppliers = suppliers.OrderBy(c => c.SupplierAddress).ToList();
                    break;
                case 4:
                    suppliers = suppliers.OrderBy(c => c.SupplierEmail).ToList();
                    break;
                case 5:
                    suppliers = suppliers.OrderBy(c => c.SupplierPhoneNumber).ToList();
                    break;
                case 6:
                    suppliers = suppliers.OrderBy(c => c.SupplierWebsite).ToList();
                    break;
                default:
                    break;
            }
            if (sortColumnInDescendingOrder)
            {
                suppliers.Reverse();
            }

            DataTable sortedDataTable = ConvertSupplierListToDataTable(suppliers);

            sortedDataTable.Columns[columnToSortBy].ColumnName = Misc.ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);
            return sortedDataTable;
        }
        public static void UpdateSupplier(int supplierID, string SupplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Program.Warehouse.SupplierTable.Update(supplierID, SupplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);
        }

        public static DataTable DeleteSupplier(DataTable dataTable, int SupplierID)
        {
            Program.Warehouse.SupplierTable.Delete(SupplierID);
            List<Supplier> suppliers = ConvertDataTableToSupplierList(dataTable);
            suppliers.RemoveAll(c => c.SupplierID == SupplierID);

            return ConvertSupplierListToDataTable(suppliers);

        }
    }
}