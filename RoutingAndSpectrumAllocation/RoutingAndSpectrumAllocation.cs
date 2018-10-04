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
    public abstract class RoutingAndSpectrumAllocation : IRoutingAndSpectrumAllocation
    {
        protected IGraphInputReader InputReader { get; set; }
        protected IProgramLogger InfoLogger { get; set; }
        protected ILogger StorageLogger { get; set; }
        protected IRSATableFill RSATableFill { get; set; }
        protected int supplied = 0;

        public RoutingAndSpectrumAllocation(
            IGraphInputReader inputReader, 
            IProgramLogger infologger, 
            ILogger storageLogger, 
            IRSATableFill RSATableFill)
        {
            InputReader = inputReader;
            InfoLogger = infologger;
            StorageLogger = storageLogger;
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

        protected abstract Task ApplyRSA(Graph graph, List<Demand> demands, int numberOfLinkChannels);

        protected virtual List<AvailableSlot> GetAvailableTableSlots(Graph graph, GraphPath path, RSATable table)
        {
            List<AvailableSlot> availableSlots = new List<AvailableSlot>();
            List<GraphLink> pathLinks = path.ToLinks(graph.Links);

            foreach (GraphLink link in pathLinks)
            {
                AvailableSlot element = new AvailableSlot();
                element.Link = link;
                element.Availables = new List<int>(table.Table[link.GetLinkId()].Where(r => r.Value.Values.Count == 0).Select(r => r.Key).ToList());
                availableSlots.Add(element);
            }

            return availableSlots;
        }

        private static List<Demand> GetDemands(Graph graph)
        {
            DemandGenerator generator = new DemandGenerator(graph.Links);
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
