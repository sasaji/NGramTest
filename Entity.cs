using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGramTest
{
    public class Entity
    {
        public string Id;
        public string Name;
        public Dictionary<string, object> Properties = new Dictionary<string, object>();
    }
}
