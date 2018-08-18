using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.FileLogger;
using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.InfoLoggers;
using RoutingAndSpectrumAllocation.InputReaders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation
{
    public class RoutingAndSpectrumAllocation : IRoutingAndSpectrumAllocation
    {
        IGraphInputReader InputReader { get; set; }
        IInfoLogger InfoLogger { get; set; }
        IFileLogger FileLogger { get; set; }

        public RoutingAndSpectrumAllocation(IGraphInputReader inputReader, IInfoLogger infologger, IFileLogger fileLogger)
        {
            InputReader = inputReader;
            InfoLogger = infologger;
            FileLogger = fileLogger;
        }

        public async Task Start(string readNodesPath, string readLinksPath)
        {
            await InfoLogger.LogInformation($"Starting RoutingAndSpectrumAllocation");
            await InfoLogger.LogInformation($"Nodes Path: \"{readNodesPath}\"");
            await InfoLogger.LogInformation($"Links Path: \"{readLinksPath}\"");

            Graph graph = ReadGraph(readNodesPath, readLinksPath);

            await FileLogger.WriteLog("graph", graph);

            List<Demand> demands = GetDemands(graph);

            await FileLogger.WriteLog("demands", graph);


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
