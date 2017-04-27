using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Business
{
    public class TopoSortItem
    {
        public uint Id { get; set; }
        public List<uint> Dependencies { get; set; }

        public TopoSortItem(uint id, List<uint> dependents)
        {
            this.Id = id;
            this.Dependencies = dependents;
        }
        public static List<TopoSortItem> BuildGraph(Dictionary<uint, List<uint>> graph)
        {
            List<TopoSortItem> items = new List<TopoSortItem>();
            foreach (uint key in graph.Keys)
                items.Add(new TopoSortItem(key, graph[key]));

            return items;
        }
    }
}
