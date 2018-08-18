using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.Graphs
{
    public interface IPathSearcher
    {
        List<GraphPath> GetPaths(Graph graph, GraphNode nodeFrom, GraphNode nodeTo, int numberOfPaths);
    }
}
