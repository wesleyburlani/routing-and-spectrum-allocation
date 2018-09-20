using RoutingAndSpectrumAllocation.Demands;
using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.ModulationFormarts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.RSA
{
    class FirstFitRMLSATableFill : IRSATableFill
    {
        public bool FillDemandOnTable(ref RSATable table, Graph graph, Demand demand, GraphPath path, List<AvailableSlot> availableSlots, bool protection = false)
        {
            List<GraphLink> pathLinks = path.ToLinks(graph.Links);

            double totalDistance = pathLinks.Sum(r => r.Length);
            IModulationFormat format = null;
            int multiplier = 1;
            do
            {
                format = GetModulationFormat(demand, totalDistance, multiplier);
                multiplier++;
            } while (format == null);


            int numberOfSlots = (int)Math.Ceiling(demand.DemandInGBps / table.LinkCapacity);

            List<List<int>> emptySlots = new List<List<int>>();
            foreach (var column in table.Table.Keys)
            {
                var reference = availableSlots.FirstOrDefault(r => r.Link.GetLinkId() == column);
                if (reference != null)
                    emptySlots.Add(new List<int>(reference.Availables));
            }

            List<int> intersection = emptySlots.First();
            foreach (var list in emptySlots.Skip(1))
                intersection = intersection.Intersect(list).ToList();

            List<int> indexesToFill = GetIndexesToFill(intersection, numberOfSlots);

            if (indexesToFill == null)
                return false;

            foreach (GraphLink link in pathLinks)
                foreach (var slot in indexesToFill)
                {
                    table.Table[link.GetLinkId()][slot].IsProtectionDemand = protection;
                    table.Table[link.GetLinkId()][slot].Values.Add(demand.Id.ToString());
                    table.Table[link.GetLinkId()][slot].ModulationFormat = format;
                }

            return true;
        }

        private IModulationFormat GetModulationFormat(Demand demand, double distance, int multiplier)
        {
            List<IModulationFormat> formats = new List<IModulationFormat>()
            {
                new BPKS(),
                new QPSK(),
                new F8QAM(),
                new F16QAM(),
                new F32QAM(),
                new F64QAM()
            };

            IModulationFormat format = formats.Where(r => distance <= r.GetReachInKm()*multiplier).OrderBy(r => r.GetReachInKm()).FirstOrDefault();
            return format;
        }

        private List<int> GetIndexesToFill(List<int> intersection, int numberOfSlots, int it = 0)
        {
            if (intersection.Count <= it)
                return null;

            if (intersection.Count() - it < numberOfSlots)
                return null;

            List<int> list = new List<int>();
            list.Add(intersection[it]);

            if (list.Count() == numberOfSlots)
                return list;

            for (int i = it + 1; i < intersection.Count(); i++)
            {
                if (list.Last() == intersection[i] - 1)
                    list.Add(intersection[i]);
                else
                    break;

                if (list.Count() == numberOfSlots)
                    return list;
            }

            return GetIndexesToFill(intersection, numberOfSlots, ++it);
        }
    }
}
