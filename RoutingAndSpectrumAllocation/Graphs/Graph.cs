using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.Graphs
{
    [Serializable]
    public class Graph
    {
        public Graph(List<GraphNode> nodes, List<GraphLink> links)
        {
            Nodes = nodes;
            Links = links;
        }

        public List<GraphNode> Nodes { get; set; }
        public List<GraphLink> Links { get; set; }

        public void ChangeLinkCost(string nodeFrom, string nodeTo, long cost)
        {
            foreach (var link in this.Links.Where(r => r.From == nodeFrom && r.To == nodeTo))
                link.Cost = cost;
        }

        public void RemoveLink(string nodeFrom, string nodeTo)
        {
            Links.RemoveAll(r => r.From == nodeFrom && r.To == nodeTo);
        }
    }
}
