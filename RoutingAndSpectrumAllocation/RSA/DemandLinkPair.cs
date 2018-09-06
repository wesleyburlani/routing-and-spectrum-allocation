using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;

namespace RoutingAndSpectrumAllocation.RSA
{
    public class DemandLinkPair
    {
        public DemandLinkPair(Demand demand, GraphPath path)
        {
            Demand = demand;
            Path = path;
        }

        public DemandLinkPair()
        {

        }

        public Demand Demand { get; set; }
        public GraphPath Path { get; set; }
    }
}
