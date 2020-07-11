using System;
using System.Collections.Generic;

namespace NGramTest
{
    public static class Utility
    {
        /// <summary>
        /// もしかして…
        /// </summary>
        /// <param name="s"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetPossibility(string s, IEnumerable<string> source)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (source == null) throw new ArgumentNullException("source");

            List<string> possibilityList = new List<string>();
            int mincost = int.MaxValue;
            string moshikashite = null;
            foreach (string t in source) {
                int cost = LevenshteinDistance.Compute(s, t);
                if (cost == 0) return t;
                if (cost <= mincost) {
                    mincost = cost;
                    possibilityList.Add(t);
                }
            }

            decimal k = -1m;
            foreach (string t in possibilityList) {
                decimal result = NGram.CompareBigram(s, t);
                if (k == -1m || result > k) {
                    k = result;
                    moshikashite = t;
                }
            }
            return moshikashite;
        }
    }
}