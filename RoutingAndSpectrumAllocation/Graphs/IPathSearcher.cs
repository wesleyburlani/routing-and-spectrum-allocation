namespace RoutingAndSpectrumAllocation.Graphs
{
    public interface IPathSearcher
    {
        GraphPath GetPath(Graph graph, GraphNode nodeFrom, GraphNode nodeTo);
    }
}
