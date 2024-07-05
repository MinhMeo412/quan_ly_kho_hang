using System.Data;

namespace WarehouseManager.Core.Utility
{
    public static class SortDataTable
    {
        public static DataTable BySearchTerm(DataTable dataTable, string searchTerm)
        {
            string similarityColumnName = "deaed214aad8be477c562a849368cd6d23c38641eccf45b69cc99f2e63d01d43";

            DataTable sorted = dataTable.Copy();
            sorted.Columns.Add(similarityColumnName, typeof(double));

            // Calculate similarity scores
            foreach (DataRow row in sorted.Rows)
            {
                double[] similarityValues = new double[sorted.Columns.Count];
                for (int i = 0; i < sorted.Columns.Count; i++)
                {
                    similarityValues[i] = Misc.JaccardSimilarity($"{row[i]}", searchTerm);
                }
                row[similarityColumnName] = similarityValues.Max();
            }

            sorted = ByColumn(sorted, sorted.Columns.Count - 1, true);
            sorted = ClearDirectionArrow(sorted);
            sorted.Columns.Remove(similarityColumnName); 

            return sorted;
        }


        public static DataTable ByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            dataTable = ClearDirectionArrow(dataTable);

            DataView dataView = new DataView(dataTable);

            string direction = "ASC";
            if (sortColumnInDescendingOrder)
            {
                direction = "DESC";
            }

            // cái dataView.Sort sẽ bị hỏng nếu như tên cột có dấu phẩy
            // nên trước khi dùng cái sort đấy phải đổi tên cột sang một tên tạm để chắc chắn không có dấu phẩy
            string temporaryColumnName = "20e51678409043748bc7d98990f1044953aa8852dcc9d2f79b886d4454b62613";
            string realColumnName = dataTable.Columns[columnToSortBy].ColumnName;
            dataTable.Columns[columnToSortBy].ColumnName = temporaryColumnName;

            dataView.Sort = $"{temporaryColumnName} {direction}";

            // sort xong rồi thì đổi về tên cũ
            dataTable.Columns[columnToSortBy].ColumnName = realColumnName;

            DataTable sortedDataTable = dataView.ToTable();

            sortedDataTable.Columns[columnToSortBy].ColumnName = ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);

            return sortedDataTable;
        }

        public static DataTable ClearDirectionArrow(DataTable dataTable)
        {
            DataTable clearedDataTable = dataTable.Copy();

            string upwardsArrow = "\u25B2";
            string downwardsArrow = "\u25BC";
            for (int i = 0; i < clearedDataTable.Columns.Count; i++)
            {
                if (clearedDataTable.Columns[i].ColumnName.Contains(upwardsArrow) || clearedDataTable.Columns[i].ColumnName.Contains(downwardsArrow))
                {
                    clearedDataTable.Columns[i].ColumnName = clearedDataTable.Columns[i].ColumnName.Replace(upwardsArrow, "");
                    clearedDataTable.Columns[i].ColumnName = clearedDataTable.Columns[i].ColumnName.Replace(downwardsArrow, "");
                    clearedDataTable.Columns[i].ColumnName = clearedDataTable.Columns[i].ColumnName.Substring(0, clearedDataTable.Columns[i].ColumnName.Length - 1);
                }
            }
            return clearedDataTable;
        }

        private static string ShowCurrentSortingDirection(string currentText, bool sortColumnInDescendingOrder)
        {
            string newText = currentText;

            string upwardsArrow = "\u25B2";
            string downwardsArrow = "\u25BC";

            if (sortColumnInDescendingOrder)
            {
                if (currentText.Contains(upwardsArrow))
                {
                    newText = newText.Replace(upwardsArrow, downwardsArrow);
                }

                if (!currentText.Contains(downwardsArrow))
                {
                    newText = $"{newText} {downwardsArrow}";
                }
            }

            if (!sortColumnInDescendingOrder)
            {
                // sort in ascending order
                if (currentText.Contains(downwardsArrow))
                {
                    newText = newText.Replace(downwardsArrow, upwardsArrow);
                }

                if (!currentText.Contains(upwardsArrow))
                {
                    newText = $"{newText} {upwardsArrow}";
                }
            }

            return newText;
        }
    }
}