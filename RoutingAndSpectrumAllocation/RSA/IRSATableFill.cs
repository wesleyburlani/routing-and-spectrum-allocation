using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;

namespace RoutingAndSpectrumAllocation.RSA
{
    public interface IRSATableFill
    {
        bool FillDemandOnTable(ref RSATable table, Graph graph, Demand demand, GraphPath path);
    }
}
