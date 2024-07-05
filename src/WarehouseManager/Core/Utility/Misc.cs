namespace WarehouseManager.Core.Utility
{
    public static class Misc
    {
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
    }
}