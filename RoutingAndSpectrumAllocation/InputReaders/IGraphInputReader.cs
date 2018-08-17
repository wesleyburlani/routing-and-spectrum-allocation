using RoutingAndSpectrumAllocation.Graphs;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.InputReaders
{
    public interface IGraphInputReader
    {
        List<GraphLink> GetLinks(string path);
        List<GraphNode> GetNodes(string path);
    }
}
