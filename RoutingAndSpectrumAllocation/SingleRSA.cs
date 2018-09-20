using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.InputReaders;
using RoutingAndSpectrumAllocation.Loggers;
using RoutingAndSpectrumAllocation.RSA;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation
{
    public class SingleRSA : RoutingAndSpectrumAllocation
    {
        IPathSearcher PathSearcher { get; set; }

        public SingleRSA(
            IGraphInputReader inputReader, 
            IProgramLogger infologger, 
            ILogger storageLogger, 
            IPathSearcher pathSearcher,
            IRSATableFill RSATableFill) 
            : base(inputReader, infologger, storageLogger, RSATableFill)
        {
            PathSearcher = pathSearcher;
        }

        protected override async Task ApplyRSA(Graph graph, List<Demand> demands, int numberOfLinkChannels)
        {
            RSATable table = new RSATable(graph.Links, numberOfLinkChannels);

            foreach (Demand demand in demands)
            {
                GraphNode nodeFrom = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdFrom);
                GraphNode nodeTo = graph.Nodes.FirstOrDefault(r => r.Id == demand.NodeIdTo);

                await InfoLogger.LogInformation($"processing demand from {demand.NodeIdFrom} to {demand.NodeIdTo} of {demand.DemandInGBps} gbps or {demand.Slots} slots\n");

                List<GraphPath> paths = PathSearcher.GetPaths(graph, nodeFrom, nodeTo, 2);

                if (paths.Count() == 0)
                {
                    await InfoLogger.LogInformation($"path from {demand.NodeIdFrom} to {demand.NodeIdTo} not found.\n");
                    continue;
                }

                table = await FillTable(table, graph,  demand, paths);
            }

            await InfoLogger.LogInformation($"Finished\n");

            await InfoLogger.LogInformation($"Total Demands: {demands.Count}\nSupplied: {supplied}\nBlocked: {demands.Count - supplied}\n");

            await InfoLogger.LogInformation(table.ToStringTable());
        }

        private async Task<RSATable> FillTable(RSATable table, Graph graph, Demand demand, List<GraphPath> paths)
        {
            bool filled = false;
            foreach (var path in paths)
            {
                await InfoLogger.LogInformation($"trying path: {string.Join("->", path.Path)}  distance: {path.ToLinks(graph.Links).Sum(r => r.Length)}");

                List<AvailableSlot> availableTableSlots = GetAvailableTableSlots(graph, path, table);

                if (RSATableFill.FillDemandOnTable(ref table, graph, demand, path, availableTableSlots))
                {
                    filled = true;
                    await InfoLogger.LogInformation($"demand supplied\n");
                    await InfoLogger.LogInformation(table.ToStringTable());
                    supplied++;
                    break;
                }
            }

            if (filled == false) 
                await InfoLogger.LogInformation($"It's not possible to supply demand of {demand.Slots}  from {demand.NodeIdFrom} to {demand.NodeIdTo}\n");
            return table;
        }
    }
}
