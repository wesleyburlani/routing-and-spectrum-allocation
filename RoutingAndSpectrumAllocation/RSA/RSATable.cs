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
        public double LinkCapacity { get; set; }

        public RSATable(List<GraphLink> links, int numberOfLinkChannels, double linkCapacity = 12.5)
        {
            Links = links;
            NumberOfLinkChannels = numberOfLinkChannels;
            Table = new Dictionary<string, Dictionary<int, RSATableElement>>();
            LinkCapacity = linkCapacity;
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
            int maxDemands = GetMaxNumberOfDemands();
            int maxValueLength = GetMaxColumnSpacing();
            int maxLength = list.Select(r => r.Count()).Max();

            string table = "";
            for (int y = 0; y < maxDemands; y++)
                table = PrintFirstTableLine(maxValueLength, maxLength, table);
            
            table += "\n";

            for (int i = 0; i < list.Count(); i++)
            {
                for (int y = 0; y < maxDemands; y++)
                {
                    table = PrintLineHashId(list, maxLength, table, i);
                    table = PrintLineValues(list, maxValueLength, table, i, y);
                }
                table += "\n";
            }
            return table;
        }

        private string PrintLineValues(List<string> list, int maxValueLength, string table, int i, int y)
        {
            for (int j = 0; j < NumberOfLinkChannels; j++)
            {
                int lenght = Table[list[i]][j].Values.Count;
                string modulation = Table[list[i]][j].ModulationFormat == null ? "" : "-" + Table[list[i]][j].ModulationFormat.GetType().Name;
                string value = y >= lenght ? "0" : Table[list[i]][j].Values[y] + modulation + (Table[list[i]][j].IsProtectionDemand ? "*" : "");
                table += value;
                for (int x = 0; x < maxValueLength - value.Count(); x++)
                    table += " ";
            }
            return table;
        }

        private static string PrintLineHashId(List<string> list, int maxLength, string table, int i)
        {
            table += $"{list[i]}";
            for (int k = 0; k < maxLength - list[i].Count(); k++)
                table += " ";
            table += "\t";
            return table;
        }

        private string PrintFirstTableLine(int maxValueLength, int maxLength, string table)
        {
            for (int k = 0; k < maxLength; k++)
                table += " ";
            table += "\t";

            for (int j = 0; j < NumberOfLinkChannels; j++)
            {
                table += j;
                for (int x = 0; x < maxValueLength - NumberOfLinkChannels.ToString().Length; x++)
                    table += " ";
            }

            return table;
        }

        private int GetMaxNumberOfDemands()
        {
            return Table.Values.Select(r => r.Values.Select(y => y.Values.Count()).Max()).Max();
        }

        private int GetMaxColumnSpacing()
        {
            int maxValueLength = 0;
            foreach (var e in Table.Values)
                foreach (var r in e.Values)
                    foreach (var c in r.Values)
                        if (maxValueLength < c.Length + 7)
                            maxValueLength = c.Length + 7;
            maxValueLength += 2;
            return maxValueLength;
        }
    }
}
