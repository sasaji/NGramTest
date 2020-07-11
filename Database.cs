using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace NGramTest
{
    class Database
    {
        private List<Entity> entities = new List<Entity>();
        private NGram ngram = null;

        private Database()
        {
        }

        public static Database Create()
        {
            Database database = new Database();
            database.Load();
            database.ngram = NGram.Create(database.entities);
            return database;
        }

        public IEnumerable<Entity> GetAllEntities()
        {
            return entities;
        }

        public IEnumerable<Entity> GetMatchedEntities(string input)
        {
            /*
            return from dataItem in dataItems
                   let ng = ngram.Compare(input.Length, input, dataItem)
                   where ng == 1
                   orderby ng descending
                   select dataItem;
             */
            return ngram.Where(input);
        }

        public Entity GetPossibleEntity(string input)
        {
            List<Entity> possibilities = new List<Entity>();
            int mincost = int.MaxValue;
            Entity possibility = null;
            foreach (Entity t in entities) {
                int cost = LevenshteinDistance.Compute(input, t.Name);
                if (cost == 0) return t;
                if (cost <= mincost) {
                    mincost = cost;
                    possibilities.Add(t);
                }
            }

            decimal k = -1m;
            foreach (Entity t in possibilities) {
                decimal result = ngram.CompareBigram(input, t.Name);
                if (k == -1m || result > k) {
                    k = result;
                    possibility = t;
                }
            }
            return possibility;
        }

        private void Load()
        {
            /*
            entities = new List<Entity>() {
                new Entity { Name = "クロワッサン" },
                new Entity { Name = "クリイム" },
                new Entity { Name = "クレーム" },
                new Entity { Name = "クライム" },
                new Entity { Name = "コロネパン" },
                new Entity { Name = "クリーナ" },
                new Entity { Name = "リクーム" },
                new Entity { Name = "クリームパン" },
                new Entity { Name = "クローバースタジオ" },
                new Entity { Name = "クローム" },
                new Entity { Name = "クラウド" },
                new Entity { Name = "クリゴハン" },
                new Entity { Name = "クリキントン" },
                new Entity { Name = "クリームパン" },
                new Entity { Name = "クリスティーナ" },
                new Entity { Name = "クロノトリガー" }
            };
             */

            DirectoryEntry root = new DirectoryEntry("LDAP://CN=Users,DC=jbs,DC=local");
            string filter = "(&(objectCategory=person)(objectClass=user))";
            DirectorySearcher searcher = new DirectorySearcher(root, filter, new string[] {
                "samAccountName",
                "sn",
                "givenName",
                "mail",
                "title",
                "department"
            });
            using (SearchResultCollection results = searcher.FindAll()) {
                foreach (SearchResult result in results) {
                    DirectoryEntry resultEntry = result.GetDirectoryEntry();
                    Entity entity = new Entity();
                    entity.Id = resultEntry.Properties["samAccountName"][0].ToString();
                    entity.Name = resultEntry.Name;
                    if (resultEntry.Properties["sn"].Count > 0)
                        entity.Properties.Add("sn", resultEntry.Properties["sn"][0]);
                    if (resultEntry.Properties["givenName"].Count > 0)
                        entity.Properties.Add("givenName", resultEntry.Properties["givenName"][0]);
                    if (resultEntry.Properties["mail"].Count > 0)
                        entity.Properties.Add("mail", resultEntry.Properties["mail"][0].ToString().Split('@')[0].Replace(".", string.Empty));
                    if (resultEntry.Properties["title"].Count > 0)
                        entity.Properties.Add("title", resultEntry.Properties["title"][0]);
                    if (resultEntry.Properties["department"].Count > 0)
                        entity.Properties.Add("department", resultEntry.Properties["department"][0]);
                    entities.Add(entity);
                }
            }
        }
    }
}
