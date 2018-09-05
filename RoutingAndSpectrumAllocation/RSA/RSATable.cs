using RoutingAndSpectrumAllocation.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.RSA
{
    [Serializable]
    public class RSATable
    {
        public Dictionary<string, Dictionary<int, RSATableElement>> Table { get; set; }
        private List<GraphLink> Links { get; set; }
        private int NumberOfLinkChannels { get; set; }

        public RSATable(List<GraphLink> links, int numberOfLinkChannels)
        {
            Links = links;
            NumberOfLinkChannels = numberOfLinkChannels;
            Table = new Dictionary<string, Dictionary<int, RSATableElement>>();
            
            foreach(GraphLink link in Links)
            {
                Table[link.GetLinkId()] = new Dictionary<int, RSATableElement>();

                for (int i = 0; i < numberOfLinkChannels; i++)
                    Table[link.GetLinkId()][i] = new RSATableElement();
            }
        }

        public string ToStringTable()
        {
            List<string> list = Table.Keys.ToList();
            int maxDemands = Table.Values.Select(r => r.Values.Select(y => y.Values.Count()).Max()).Max();
            int maxLength = list.Select(r => r.Count()).Max();

            string table = "";
            for (int y = 0; y < maxDemands; y++)
            {
                for (int k = 0; k < maxLength; k++)
                    table += " ";
                table += "\t";

                for (int j = 0; j < NumberOfLinkChannels; j++)
                    table += j + "\t";
            }
            table += "\n";

            for (int i = 0; i < list.Count(); i++)
            {
                for (int y = 0; y < maxDemands; y++)
                {
                    table += $"{list[i]}";
                    for (int k = 0; k < maxLength - list[i].Count(); k++)
                        table += " ";
                    table += "\t";
                    for (int j = 0; j < NumberOfLinkChannels; j++)
                    {
                        int lenght = Table[list[i]][j].Values.Count;
                        if (y >= lenght)
                            table += "0\t";
                        else
                            table += (Table[list[i]][j].Values[y]) + (Table[list[i]][j].IsProtectionDemand ? "*" : "") + "\t";
                    }
                }
                table += "\n";
            }
            return table;
        }   
    }
}
