using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace NGramTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = Database.Create();

            while (true) {
                Console.WriteLine("何かキーワードを入力してください... ( exit で終了します。)");
                var input = Console.ReadLine();
                if (input == "exit") break;
                if (input == "list") {
                    database.GetAllEntities().ToList().ForEach(i => Console.WriteLine(i.Name));
                    continue;
                }

                database.GetMatchedEntities(input).ToList().ForEach(i => Console.WriteLine(i.Name));
                //Console.WriteLine(database.GetPossibility(input).Name);
                Console.WriteLine();
            }
        }
    }
}
