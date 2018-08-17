using RoutingAndSpectrumAllocation.Graphs;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.InputReaders
{
    public interface IInputReader
    {
        List<GraphLink> GetLinks(string path);
        List<GraphNode> GetNodes(string path);
    }
}
