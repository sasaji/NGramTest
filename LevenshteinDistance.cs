using System;

namespace NGramTest
{
    /// <summary>
    /// レーベンシュタイン距離
    /// </summary>
    public static class LevenshteinDistance
    {
        public static int Compute(string s, string t)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (t == null) throw new ArgumentNullException("t");

            int x = s.Length;
            int y = t.Length;

            if (x == 0) return y;
            if (y == 0) return x;

            int[,] d = new int[x + 1, y + 1];

            for (int i = 0; i <= x; d[i, 0] = i++) ;
            for (int j = 0; j <= y; d[0, j] = j++) ;

            int cost = default(int);
            for (int i = 1; i <= x; i++) {
                for (int j = 1; j <= y; j++) {
                    cost = (s.Substring(i - 1, 1) != t.Substring(j - 1, 1) ? 1 : 0);
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1)
                                     , d[i - 1, j - 1] + cost);
                }
            }
            return d[x, y];
        }
    }
}