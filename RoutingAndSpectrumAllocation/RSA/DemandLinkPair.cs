using System;
using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;

namespace RoutingAndSpectrumAllocation.RSA
{
    public class DemandLinkPair
    {
        public DemandLinkPair()
        {

        }

        public DemandLinkPair(Demand demand, Tuple<GraphPath, GraphPath> tuplePaths)
        {
            Demand = demand;
            TuplePaths = tuplePaths;
        }

        public Demand Demand { get; set; }
        public Tuple<GraphPath, GraphPath> TuplePaths { get; set; }
    }
}
