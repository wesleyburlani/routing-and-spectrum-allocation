using RoutingAndSpectrumAllocation.Graphs;
using System.Collections.Generic;
using System.Linq;

namespace RoutingAndSpectrumAllocation.RSA
{
    public class RSATable
    {
        public Dictionary<string, Dictionary<int, bool>> Table { get; set; }
        private List<GraphLink> Links { get; set; }
        private int NumberOfLinkChannels { get; set; }

        public RSATable(List<GraphLink> links, int numberOfLinkChannels)
        {
            Links = links;
            NumberOfLinkChannels = numberOfLinkChannels;
            Table = new Dictionary<string, Dictionary<int, bool>>();
            
            foreach(GraphLink link in Links)
            {
                Table[link.GetLinkId()] = new Dictionary<int, bool>();

                for (int i = 0; i < numberOfLinkChannels; i++)
                    Table[link.GetLinkId()][i] = false;
            }
        }

        public string ToStringTable()
        {
            List<string> list = Table.Keys.ToList();
            int maxLength = list.Select(r => r.Count()).Max();

            string table = "";
            for (int k = 0; k < maxLength ; k++)
                table += " ";
            table += "\t";
            for (int j = 0; j < NumberOfLinkChannels; j++)
                table += j + "\t";
            table += "\n";

            for (int i=0; i < list.Count(); i++)
            {
                table += $"{list[i]}";
                for (int k = 0; k < maxLength - list[i].Count(); k++)
                    table += " ";
                table+= "\t";
                for (int j = 0; j < NumberOfLinkChannels; j++)
                    table += (Table[list[i]][j] ? "1" : "0") + "\t";
                table += "\n";
            }
            return table;
        }   
    }
}
