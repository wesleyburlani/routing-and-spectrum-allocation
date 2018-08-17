using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.Graphs
{
    public class Graph
    {
        public Graph(List<GraphNode> nodes, List<GraphLink> links)
        {
            Nodes = nodes;
            Links = links;
        }

        public List<GraphNode> Nodes { get; set; }
        public List<GraphLink> Links { get; set; }
    }
}
