using System;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.Graphs
{
    public interface IDisjointedPathPairSearcher
    {
        List<Tuple<GraphPath, GraphPath>> GetDisjointedPaths(Graph graph, GraphNode nodeFrom, GraphNode nodeTo, int numberOfMainPaths, int numberOfSecundaryPaths);
    }
}
