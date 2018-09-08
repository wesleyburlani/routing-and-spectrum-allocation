using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.RSA
{
    public interface IRSATableFill
    {
        bool FillDemandOnTable(ref RSATable table, Graph graph, Demand demand, GraphPath path, List<AvailableSlot> availableSlots, bool protection = false);
    }
}
