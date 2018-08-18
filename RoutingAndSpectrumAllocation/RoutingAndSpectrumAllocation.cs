using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Loggers;
using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.InputReaders;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Data;

namespace RoutingAndSpectrumAllocation
{
    public class RoutingAndSpectrumAllocation : IRoutingAndSpectrumAllocation
    {
        IGraphInputReader InputReader { get; set; }
        IInfoLogger InfoLogger { get; set; }
        IStorageLogger StorageLogger { get; set; }
        IPathSearcher PathSearcher { get; set; }

        public RoutingAndSpectrumAllocation(
            IGraphInputReader inputReader, 
            IInfoLogger infologger, 
            IStorageLogger storageLogger, 
            IPathSearcher pathSearcher)
        {
            InputReader = inputReader;
            InfoLogger = infologger;
            StorageLogger = storageLogger;
            PathSearcher = pathSearcher;
        }

        public async Task Start(string readNodesPath, string readLinksPath)
        {
            await InfoLogger.LogInformation($"Starting RoutingAndSpectrumAllocation");
            await InfoLogger.LogInformation($"Nodes Path: \"{readNodesPath}\"");
            await InfoLogger.LogInformation($"Links Path: \"{readLinksPath}\"");

            Graph graph = ReadGraph(readNodesPath, readLinksPath);

            await StorageLogger.WriteLog("graph", graph);

            List<Demand> demands = GetDemands(graph);

            await StorageLogger.WriteLog("demands", graph);

            await ApplyRSA(graph, demands);
        }

        private Task ApplyRSA(Graph graph, List<Demand> demands)
        { 
            foreach(Demand demand in demands)
            {
                GraphNode nodeFrom = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdFrom);
                GraphNode nodeTo = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdTo);

                List<GraphPath> path = PathSearcher.GetPaths(graph, nodeFrom, nodeTo, 1);

                if (path.Count() == 0)
                {
                    InfoLogger.LogInformation($"path from {demand.NodeIdFrom} to {demand.NodeIdTo} not found.");
                    continue;
                }

                InfoLogger.LogInformation("ok");


            }

            return Task.CompletedTask;
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
