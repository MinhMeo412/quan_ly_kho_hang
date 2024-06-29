namespace WarehouseManager.Core.Utility
{
    public static class JaccardIndex
    {
        public static double Similarity(string text1, string text2)
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