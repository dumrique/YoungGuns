using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoungGuns.DataAccess;
using YoungGuns.Shared;

namespace YoungGuns.Business
{
    public class TopoListBuilder
    {
        public async static Task BuildAndStoreTopoList(string taxSystemId, Dictionary<uint, List<uint>> graph)
        {
            // topo sort
            var sorted = new List<uint>();
            var visited = new Dictionary<uint, bool>();

            foreach (var key in graph.Keys)
                Visit(graph, key, sorted, visited);

            // "sorted" is now the topo list
            //      so store it in table storage
            await new DbHelper().SaveTopoList(new TaxSystemTopoFieldList() { Id = taxSystemId, TopoList = sorted });
        }

        public static void Visit(Dictionary<uint, List<uint>> graph, uint itemId, List<uint> sorted, Dictionary<uint, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(itemId, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[itemId] = true;

                var dependencies = graph[itemId];
                if (dependencies != null)
                {
                    foreach (var depId in dependencies)
                    {
                        Visit(graph, depId, sorted, visited);
                    }
                }

                visited[itemId] = false;
                sorted.Add(itemId);
            }
        }
    }
}
