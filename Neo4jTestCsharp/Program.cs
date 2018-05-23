using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

namespace Neo4jTestCsharp
{
    class Program
    {
        private static IDriver _driver;
        static void Main(string[] args)
        {
            _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "admin"));

            //var message = "test";

            //using (var session = _driver.Session())
            //{
            //    var greeting = session.WriteTransaction(tx =>
            //    {
            //        var result = tx.Run("MATCH (n) return n");
            //        return result.First()[0].As<string>();
            //    });
            //    Console.WriteLine(greeting);
            //    Console.ReadKey();
            //}

            var statementText = "MATCH (a:Person)-[:InLove]->(b:Person) RETURN a.name as name, collect(b.name) as count";
            var statementParameters = new Dictionary<string, object> { { "limit", 100 } };

            var nodes = new List<NodeResult>();
            var relationships = new List<object>();

            using (var session = _driver.Session())
            {
                var result = session.Run(statementText, statementParameters);

                var i = 0;

                foreach (var record in result)
                {
                    var target = i;
                    nodes.Add(new NodeResult { title = record["name"].As<string>(), label = "Person" });
                    i += 1;

                    var castMembers = record["count"].As<List<string>>(); // lista osób, które osoba a kocha
                    //var castMembers = record["cast"].As<List<string>>();
                    //foreach (var castMember in castMembers)
                    //{
                    //    var source = nodes.FindIndex(c => c.title == castMember);
                    //    if (source == -1)
                    //    {
                    //        nodes.Add(new NodeResult { title = castMember, label = "actor" });
                    //        source = i;
                    //        i += 1;
                    //    }
                    //    relationships.Add(new { source, target });
                    //}

                    // dodawanie, usuwanie, modyfikacja
                }
            }
        }

        public class NodeResult
        {
            public string title { get; set; }
            public string label { get; set; }
        }
    }
}
