using RoutingAndSpectrumAllocation.Graphs;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.RSA
{
    public class AvailableSlot
    {
        public GraphLink Link { get; set; }
        public List<int> Availables { get; set; }
    }
}
