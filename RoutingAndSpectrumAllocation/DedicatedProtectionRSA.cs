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
    public class DedicatedProtectionRSA : RoutingAndSpectrumAllocation
    {
        IDisjointedPathPairSearcher DisjointedPathPairSearcher { get; set; }

        public DedicatedProtectionRSA(
            IGraphInputReader inputReader, 
            IProgramLogger infologger, 
            ILogger storageLogger,
            IDisjointedPathPairSearcher disjointedPathPairSearcher,
            IRSATableFill RSATableFill)
            : base(inputReader, infologger, storageLogger, RSATableFill)
        {
            DisjointedPathPairSearcher = disjointedPathPairSearcher;
        }

        protected override async Task ApplyRSA(Graph graph, List<Demand> demands, int numberOfLinkChannels)
        {
            RSATable table = new RSATable(graph.Links, numberOfLinkChannels);

            foreach (Demand demand in demands)
            {
                GraphNode nodeFrom = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdFrom);
                GraphNode nodeTo = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdTo);

                await InfoLogger.LogInformation($"processing demand from {demand.NodeIdFrom} to {demand.NodeIdTo} of {demand.DemandInGBps} gbps or {demand.Slots} slots\n");

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

                await InfoLogger.LogInformation($"trying main path: {string.Join("->", path.Item1.Path)} distance: {path.Item1.ToLinks(graph.Links).Sum(r => r.Length)}");

                List<AvailableSlot> availableTableSlots = GetAvailableTableSlots(graph, path.Item1, tableMemory);

                if (RSATableFill.FillDemandOnTable(ref tableMemory, graph, demand, path.Item1, availableTableSlots))
                {
                   availableTableSlots = GetAvailableTableSlots(graph, path.Item2, tableMemory);

                    await InfoLogger.LogInformation($"trying secundary path: {string.Join("->", path.Item2.Path)} distance: {path.Item2.ToLinks(graph.Links).Sum(r => r.Length)}");

                    if (RSATableFill.FillDemandOnTable(ref tableMemory, graph, demand, path.Item2, availableTableSlots, true))
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
    }
}
