using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.Graphs
{
    [Serializable]
    public class GraphPath
    {
        public GraphPath(IEnumerable<string> path = null)
        {
            path = path ?? new List<string>();
            Path = new List<string>();
            Path.AddRange(path);
        }

        public List<string> Path { get; set; }

        public List<GraphLink> ToLinks(List<GraphLink> allGraphLinks)
        {
            List<GraphLink> links = new List<GraphLink>();

            for (int i = 0; i < Path.Count-1; i++)
            {
                var node1 = Path[i];
                var node2 = Path[i + 1];

                var link = allGraphLinks.FirstOrDefault(r => (r.From == node1 && r.To == node2) || (r.To == node1 && r.From == node2));
                links.Add(link);
            }

            return links;
        }
    }
}
