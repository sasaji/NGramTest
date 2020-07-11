using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGramTest
{
    public class DataItem
    {
        public int LD;
        public decimal Unigram;
        public decimal Bigram;
        public decimal Trigram;
        public decimal Ngram;
        public string Value;

        public DataItem(int ld, decimal unigram, decimal bigram, decimal ngram, string value)
        {
            LD = ld;
            Unigram = unigram;
            Bigram = bigram;
            Ngram = ngram;
            Value = value;
        }
    }
}
