using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.InputReaders;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation
{
    public class RoutingAndSpectrumAllocation : IRoutingAndSpectrumAllocation
    {
        IInputReader InputReader { get; set; }

        public RoutingAndSpectrumAllocation(IInputReader inputReader)
        {
            InputReader = inputReader;
        }

        public void Start(string readNodesPath, string readLinksPath)
        {
            Graph graph = ReadGraph(readNodesPath, readLinksPath);
        }

        private Graph ReadGraph(string readNodesPath, string readLinksPath)
        {
            List<GraphNode> nodes = InputReader.GetNodes(readNodesPath);
            List<GraphLink> links = InputReader.GetLinks(readLinksPath);
            return new Graph(nodes, links);
        }
    }
}
