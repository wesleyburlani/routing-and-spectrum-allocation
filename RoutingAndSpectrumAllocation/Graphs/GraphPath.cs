using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.Graphs
{
    public class GraphPath
    {
        public GraphPath(IEnumerable<string> path = null)
        {
            path = path ?? new List<string>();
            Path = new List<string>();
            Path.AddRange(path);
        }

        public List<string> Path { get; set; }
    }
}
