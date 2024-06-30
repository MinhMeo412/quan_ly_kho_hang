namespace WarehouseManager.Core.Utility
{
    public static class Misc
    {
        // trả về giá trị nhỏ nhất trong một dãy double
        public static double MaxDouble(params double[] values)
        {
            return values.Max();
        }

        // trả về độ giống nhau giữa 2 chuỗi (double). 1.0 là y hệt, 0.0 là khác hoàn toàn.
        public static double JaccardSimilarity(string text1, string text2)
        {
            var set1 = new HashSet<char>(text1);
            var set2 = new HashSet<char>(text2);

            var intersection = new HashSet<char>(set1);
            intersection.IntersectWith(set2);

            var union = new HashSet<char>(set1);
            union.UnionWith(set2);

            return (double)intersection.Count / union.Count;
        }

        // Thêm icon mũi tên để chỉ hướng sort của cột
        public static string ShowCurrentSortingDirection(string currentText, bool sortColumnInDescendingOrder)
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