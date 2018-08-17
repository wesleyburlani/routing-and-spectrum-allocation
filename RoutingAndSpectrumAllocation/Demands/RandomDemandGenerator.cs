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

            int iterator = random.Next(1, MaxNumberOfLinks(linkIds.Count()));

            List<string> memory = new List<string>(); 
            
            for(int it = 0; it < iterator; it++)
            {
                int first = random.Next(linkIds.Count());
                int second = random.Next(linkIds.Count());
                while(second == first)
                    second = random.Next(linkIds.Count());

                string hash = first + "-" + second;
                if (memory.Contains(hash))
                    continue;

                int demand = random.Next(1, 10);
                demands.Add(new Demand(linkIds[first], linkIds[second], demand));
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

        private int MaxNumberOfLinks(int numberOfNodes)
        {
            return numberOfNodes * (numberOfNodes - 1) / 2;
        } 
    }
}
