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

        List<GraphNode> Nodes { get; set; }
        List<GraphLink> Links { get; set; }
    }
}
