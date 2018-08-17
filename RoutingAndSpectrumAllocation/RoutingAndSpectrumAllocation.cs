using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.InputReaders;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation
{
    public class RoutingAndSpectrumAllocation : IRoutingAndSpectrumAllocation
    {
        IGraphInputReader InputReader { get; set; }

        public RoutingAndSpectrumAllocation(IGraphInputReader inputReader)
        {
            InputReader = inputReader;
        }

        public void Start(string readNodesPath, string readLinksPath)
        {
            Graph graph = ReadGraph(readNodesPath, readLinksPath);
            List<Demand> demands = GetDemands(graph);


        }

        private static List<Demand> GetDemands(Graph graph)
        {
            RandomDemandGenerator generator = new RandomDemandGenerator(graph.Links);
            return generator.GetDemands();
        }

        private Graph ReadGraph(string readNodesPath, string readLinksPath)
        {
            List<GraphNode> nodes = InputReader.GetNodes(readNodesPath);
            List<GraphLink> links = InputReader.GetLinks(readLinksPath);
            return new Graph(nodes, links);
        }
    }
}
