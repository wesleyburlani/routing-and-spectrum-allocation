using RoutingAndSpectrumAllocation.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.Demands
{
    public class RandomDemandGenerator 
    {
        public RandomDemandGenerator(List<GraphLink> links)
        {
            Links = links;
        }

        List<GraphLink> Links { get; set; }

        public List<Demand> GetDemands()
        {
            List<Demand> demands = new List<Demand>();

            List<string> linkIds = GetLinkIds();

            Random random = new Random();

            int iterator = random.Next(1, GetMaximumNumberOfLinks(linkIds.Count()));

            List<string> memory = new List<string>(); 
            
            for(int it = 1; it <= iterator; it++)
            {
                int first = random.Next(0,linkIds.Count()-1);
                int second = random.Next(0,linkIds.Count()-1);
                while(second == first)
                    second = random.Next(linkIds.Count());

                string hash = first + "-" + second;
                if (memory.Contains(hash))
                    continue;

                int demand = random.Next(1, 10);
                double demandInGbps = 40.0;
                demands.Add(new Demand(it,linkIds[first], linkIds[second], demand, demandInGbps));
                memory.Add(hash);
            }

            return demands;
        }

        private List<string> GetLinkIds()
        {
            List<string> distinctIds = Links.Select(r => r.From).ToList();
            distinctIds.AddRange(Links.Select(r => r.To));
            distinctIds = distinctIds.Distinct().ToList();
            return distinctIds;
        }

        private int GetMaximumNumberOfLinks(int numberOfNodes)
        {
            return numberOfNodes * (numberOfNodes - 1) / 2;
        } 
    }
}
