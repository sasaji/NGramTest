using System;
using System.Linq;
using System.Collections.Generic;

namespace NGramTest
{
    public class NGram
    {
        private Dictionary<Entity, List<string>> ngrams = new Dictionary<Entity, List<string>>();

        private NGram()
        {
        }

        public static NGram Create(IEnumerable<Entity> dataItems)
        {
            NGram ngram = new NGram();
            ngram.CreateNGrams(dataItems);
            return ngram;
        }

        private void CreateNGrams(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities) {
                foreach (KeyValuePair<string, object> property in entity.Properties) {
                    for (int n = 1; n <= property.Value.ToString().Length; n++) {
                        for (int i = 0; i < property.Value.ToString().Length - (n - 1); i++) {
                            string ngitem = property.Value.ToString().Substring(i, n);
                            if (!ngrams.ContainsKey(entity))
                                ngrams.Add(entity, new List<string>());
                            if (!ngrams[entity].Contains(ngitem))
                                ngrams[entity].Add(ngitem);
                        }
                    }
                }
            }
            Console.WriteLine("size: " + ngrams.Sum(e => e.Value.Count));
        }

        /// <summary>
        /// Unigram
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public decimal CompareUnigram(string s, string t)
        {
            return Compare(1, s, t);
        }

        /// <summary>
        /// Bigram
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns>
        /// </returns>
        public decimal CompareBigram(string s, string t)
        {
            return Compare(2, s, t);
        }

        /// <summary>
        /// Trigram
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public decimal CompareTrigram(string s, string t)
        {
            return Compare(3, s, t);
        }

        public decimal Compare(int n, string s, string t)
        {
            var noise = new List<string>();
            for (int i = 0; i < s.Length - (n - 1); i++) {
                var ngitem = s.Substring(i, n);
                if (!noise.Contains(ngitem)) { noise.Add(ngitem); }
            }
            if (noise.Count == 0) return 0;

            int coincidence = 0;
            for (int i = 0; i < t.Length - (n - 1); i++) {
                var ngitem = t.Substring(i, n);
                if (noise.Contains(ngitem)) { coincidence++; }
            }
            return (decimal)coincidence / noise.Count;
        }

        public IEnumerable<Entity> Where(string s)
        {
            return ngrams.Where(e => e.Value.Contains(s)).Select(e => e.Key);
        }
    }
}