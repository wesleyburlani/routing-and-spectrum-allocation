using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.InputReaders;
using RoutingAndSpectrumAllocation.Loggers;
using RoutingAndSpectrumAllocation.RSA;
using RoutingAndSpectrumAllocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation
{
    public class DedicatedProtectionRSA : IRoutingAndSpectrumAllocation
    {
        IGraphInputReader InputReader { get; set; }
        IProgramLogger InfoLogger { get; set; }
        ILogger StorageLogger { get; set; }
        IDisjointedPathPairSearcher DisjointedPathPairSearcher { get; set; }
        IRSATableFill RSATableFill { get; set; }
        int supplied = 0;

        public DedicatedProtectionRSA(
            IGraphInputReader inputReader, 
            IProgramLogger infologger, 
            ILogger storageLogger,
            IDisjointedPathPairSearcher disjointedPathPairSearcher,
            IRSATableFill RSATableFill)
        {
            InputReader = inputReader;
            InfoLogger = infologger;
            StorageLogger = storageLogger;
            DisjointedPathPairSearcher = disjointedPathPairSearcher;
            this.RSATableFill = RSATableFill;
        }

        public async Task Start(string readNodesPath, string readLinksPath, int numberOfLinkChannels)
        {
            await InfoLogger.LogInformation($"Starting RoutingAndSpectrumAllocation\n");
            await InfoLogger.LogInformation($"Nodes Path: \"{readNodesPath}\"\n");
            await InfoLogger.LogInformation($"Links Path: \"{readLinksPath}\"\n");

            Graph graph = ReadGraph(readNodesPath, readLinksPath);

            await StorageLogger.WriteLog("graph", graph);

            List<Demand> demands = GetDemands(graph);

            await StorageLogger.WriteLog("demands", demands);

            await ApplyRSA(graph, demands, numberOfLinkChannels);
        }

        private async Task ApplyRSA(Graph graph, List<Demand> demands, int numberOfLinkChannels)
        {
            RSATable table = new RSATable(graph.Links, numberOfLinkChannels);

            foreach (Demand demand in demands)
            {
                GraphNode nodeFrom = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdFrom);
                GraphNode nodeTo = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdTo);

                await InfoLogger.LogInformation($"processing demand from {demand.NodeIdFrom} to {demand.NodeIdTo} with {demand.Slots} slots\n");

                List<Tuple<GraphPath, GraphPath>> paths = DisjointedPathPairSearcher.GetDisjointedPaths(graph, nodeFrom, nodeTo, 1, 1);

                if (paths.Count() == 0)
                {
                    await InfoLogger.LogInformation($"path from {demand.NodeIdFrom} to {demand.NodeIdTo} not found.\n");
                    continue;
                }

                table = await FillTable(table, graph, demand, paths);
            }

            await InfoLogger.LogInformation($"Finished\n");

            await InfoLogger.LogInformation($"Total Demands: {demands.Count}\nSupplied: {supplied}\nBlocked: {demands.Count - supplied}\n");

            await InfoLogger.LogInformation(table.ToStringTable());
        }

        private async Task<RSATable> FillTable(RSATable table, Graph graph, Demand demand, List<Tuple<GraphPath, GraphPath>> paths)
        {
            bool filled = false;
            foreach (var path in paths)
            {
                var tableMemory = table.CopyObject<RSATable>();

                await InfoLogger.LogInformation($"trying main path: {string.Join("->", path.Item1.Path)}");

                if (RSATableFill.FillDemandOnTable(ref tableMemory, graph, demand, path.Item1))
                {
                    await InfoLogger.LogInformation($"trying secundary path: {string.Join("->", path.Item2.Path)}");

                    if (RSATableFill.FillDemandOnTable(ref tableMemory, graph, demand, path.Item2))
                    {
                        filled = true;
                        await InfoLogger.LogInformation($"demand supplied\n");
                        table = tableMemory;
                        await InfoLogger.LogInformation(table.ToStringTable());
                        supplied++;
                        break;
                    }
                }
            }

            if (filled == false) 
                await InfoLogger.LogInformation($"It's not possible to supply demand of {demand.Slots}  from {demand.NodeIdFrom} to {demand.NodeIdTo}\n");
            return table;
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
