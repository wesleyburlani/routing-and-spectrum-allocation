using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.Graphs
{
    public class Dijkstra : IPathSearcher
    {
        public List<GraphPath> GetPaths(Graph graph, GraphNode nodeFrom, GraphNode nodeTo, int numberOfPaths)
        {
            List<GraphPath> paths = new List<GraphPath>();
            List<InternalNodeDijkstra> priorityList = new List<InternalNodeDijkstra>();

            GraphPath path = new GraphPath(new string[]{ nodeFrom.Id });
            Dictionary<string, int> counter = StartCounter(graph);

            priorityList.Add(new InternalNodeDijkstra(nodeFrom.Id, path, 0));

            InternalNodeDijkstra nodeDijkstra;
            do
            {
                nodeDijkstra = priorityList.First();
                priorityList = priorityList.Skip(1).ToList();

                if (nodeDijkstra.NodeId == nodeTo.Id)
                {
                    paths.Add(nodeDijkstra.Path);
                    if (paths.Count == numberOfPaths)
                        break;
                    continue;
                }

                if (++counter[nodeDijkstra.NodeId] > numberOfPaths)
                    continue;

                foreach (var neighboor in GetNodeNeighboorsIds(graph, nodeDijkstra))
                {
                    if (nodeDijkstra.Path.Path.Contains(neighboor))
                        continue;

                    path = new GraphPath(nodeDijkstra.Path.Path);
                    path.Path.Add(neighboor);
                    GraphLink link = graph.Links.FirstOrDefault(r => r.From == neighboor || r.To == neighboor);
                    priorityList.Add(new InternalNodeDijkstra(neighboor, path, nodeDijkstra.Distance + link.Cost));
                    priorityList.Sort((x,y) => x.CompareTo(y));
                }

                nodeDijkstra = null;

            } while (priorityList.Count != 0);

            return paths;
        }

        private static Dictionary<string, int> StartCounter(Graph graph)
        {
            Dictionary<string, int> counter = new Dictionary<string, int>();
            graph.Nodes.ForEach(r => counter[r.Id] = 0);
            return counter;
        }

        private static List<string> GetNodeNeighboorsIds(Graph graph, InternalNodeDijkstra nodeDijkstra)
        {
            var neighboors = graph.Links.Where(r => r.From == nodeDijkstra.NodeId).Select(r => r.To).ToList();
            neighboors.AddRange(graph.Links.Where(r => r.To == nodeDijkstra.NodeId).Select(r => r.From));
            return neighboors;
        }

        private class InternalNodeDijkstra
        {
            public InternalNodeDijkstra(string nodeId, GraphPath path, long distance)
            {
                NodeId = nodeId;
                Path = path;
                Distance = distance;
            }

            public string NodeId { get; set; }
            public GraphPath Path { get; set; }
            public long Distance { get; set; }

            public int CompareTo(object o)
            {
                InternalNodeDijkstra next = (InternalNodeDijkstra)o;
                if(Distance == next.Distance)
                    return NodeId.CompareTo(next.NodeId);
                
                if (Distance > next.Distance)
                    return 1;
                return -1;
            } 
        }
    }
}
