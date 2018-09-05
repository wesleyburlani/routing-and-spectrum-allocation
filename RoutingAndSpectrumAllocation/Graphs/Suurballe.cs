using RoutingAndSpectrumAllocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.Graphs
{
    public class Suurballe : IDisjointedPathPairSearcher
    {
        public Suurballe(IPathSearcher pathSearcher)
        {
            PathSearcher = pathSearcher;
        }

        IPathSearcher PathSearcher { get; set; }

        public List<Tuple<GraphPath, GraphPath>> GetDisjointedPaths(Graph graph, GraphNode nodeFrom, GraphNode nodeTo, int numberOfMainPaths, int numberOfSecundaryPaths)
        {
            Graph bidiretionalGraph = graph.CopyObject<Graph>();
            BuildDirectionLinks(bidiretionalGraph);

            List<Tuple<GraphPath, GraphPath>> disjointedPaths = new List<Tuple<GraphPath, GraphPath>>();

            List<GraphPath> mainPaths = PathSearcher.GetPaths(graph, nodeFrom, nodeTo, numberOfMainPaths, true);

            foreach(GraphPath mainPath in mainPaths)
            {
                Graph graphCopy = bidiretionalGraph.CopyObject<Graph>();

                for (int i = 0; i < mainPath.Path.Count - 1; i++)
                {
                    graphCopy.RemoveLink(mainPath.Path[i], mainPath.Path[i + 1]);
                    graphCopy.ChangeLinkCost(mainPath.Path[i + 1], mainPath.Path[i], 0);
                }

                List<GraphPath> secundaryPaths = PathSearcher.GetPaths(graphCopy, nodeFrom, nodeTo, numberOfSecundaryPaths, true);

                if (secundaryPaths.Count() == 0)
                    throw new Exception($"It's not possible to find two disjointed paths between {nodeFrom}->{nodeTo}");

                foreach (GraphPath secundaryPath in secundaryPaths)
                {
                    Graph ring = CreateRingSubGraph(bidiretionalGraph, mainPath);

                    ChangeLinksExistenceState(bidiretionalGraph, secundaryPath, ring);

                    List<GraphPath> path = PathSearcher.GetPaths(ring, nodeFrom, nodeTo, 1, true);

                    GraphPath resultMainPath = path.FirstOrDefault();

                    for (int i = 0; i < resultMainPath.Path.Count - 1; i++)
                    {
                        ring.RemoveLink(resultMainPath.Path[i], resultMainPath.Path[i + 1]);
                        ring.RemoveLink(resultMainPath.Path[i+1], resultMainPath.Path[i]);
                    }

                    List<GraphPath> path2 = PathSearcher.GetPaths(ring, nodeFrom, nodeTo, 1, true);

                    GraphPath resultSecundaryPath = path2.FirstOrDefault();
                    Tuple<GraphPath, GraphPath> disjointedPath = new Tuple<GraphPath, GraphPath>(resultMainPath, resultSecundaryPath);
                    disjointedPaths.Add(disjointedPath);
                }
            }

            return disjointedPaths;
        }

        private static void BuildDirectionLinks(Graph graph)
        {
            List<GraphLink> directionalLinks = new List<GraphLink>();
            foreach (var link in graph.Links)
            {
                GraphLink newLink = link.CopyObject<GraphLink>();
                newLink.From = link.To;
                newLink.To = link.From;
                directionalLinks.Add(newLink);
            }
            graph.Links.AddRange(directionalLinks);
        }

        private void ChangeLinksExistenceState(Graph graph, GraphPath secundaryPath, Graph ring)
        {
            for (int i = 0; i < secundaryPath.Path.Count - 1; i++)
            {
                GraphLink link = ring.Links.FirstOrDefault(r => r.From == secundaryPath.Path[i] && r.To == secundaryPath.Path[i + 1]);

                if (link != null)
                {
                    ring.RemoveLink(secundaryPath.Path[i], secundaryPath.Path[i + 1]);
                    ring.RemoveLink(secundaryPath.Path[i + 1], secundaryPath.Path[i]);
                }
                else
                {
                    link = graph.Links.FirstOrDefault(r => r.From == secundaryPath.Path[i] && r.To == secundaryPath.Path[i + 1]);

                    GraphLink linkCopy = link.CopyObject<GraphLink>();
                    GraphLink linkCopy2 = link.CopyObject<GraphLink>();
                    linkCopy2.From = link.To;
                    linkCopy2.To = link.From;
                    //set slots and granularity
                    ring.Links.Add(linkCopy2);
                    ring.Links.Add(linkCopy);
                }
            }
        }

        private Graph CreateRingSubGraph(Graph graph, GraphPath mainPath)
        {
            Graph ring = graph.CopyObject<Graph>();
            ring.Links = new List<GraphLink>();

            for (int i = 0; i < mainPath.Path.Count - 1; i++)
            {
                GraphLink link = graph.Links.FirstOrDefault(r => r.From == mainPath.Path[i] && r.To == mainPath.Path[i + 1]);
                GraphLink linkCopy = link.CopyObject<GraphLink>();
                //set slots and granularity
                ring.Links.Add(linkCopy);
            }
            BuildDirectionLinks(ring);
            return ring;
        }
    }
}
