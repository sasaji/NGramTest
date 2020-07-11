using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGramTest
{
    public class Candidate : IEnumerable<DataItem>
    {
        private IEnumerable<DataItem> items = new List<DataItem>();

        private Candidate()
        {
        }

        public static Candidate Create(List<string> list, string input)
        {
            Candidate candidate = new Candidate();
            candidate.items = (from s in list
                               let ld = LevenshteinDistance.Compute(input, s)
                               let unigram = NGram.CompareUnigram(input, s)
                               let bigram = NGram.CompareBigram(input, s)
                               let ngram = NGram.Compare(input.Length, input, s)
                               orderby bigram descending
                               orderby ld ascending
                               select new DataItem(ld, unigram, bigram, ngram, s)).Take(100);
            return candidate;
        }

        public IEnumerable<DataItem> GetNgramMatch()
        {
            return items.Where(i => i.Ngram == 1);
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            foreach (DataItem item in items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (DataItem item in items)
                yield return item;
        }
    }
}
