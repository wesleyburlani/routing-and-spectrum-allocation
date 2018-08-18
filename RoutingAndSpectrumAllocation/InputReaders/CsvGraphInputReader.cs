using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoutingAndSpectrumAllocation.Graphs;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoutingAndSpectrumAllocation.InputReaders
{
    public class CsvGraphInputReader : IGraphInputReader
    {
        private char CsvLineSeparator { get; set; }
        private char CsvColumnSeparator { get; set; }

        public CsvGraphInputReader(char csvLineSeparator = '\n', char csvColumnSeparator = ',')
        {
            CsvLineSeparator = csvLineSeparator;
            CsvColumnSeparator = csvColumnSeparator;
        }

        public List<GraphLink> GetLinks(string path)
        {
            string csv = ReadCsvString(path);
            string json = ParseCsvToJson(csv);
            List<GraphLink> links = JsonConvert.DeserializeObject<List<GraphLink>>(json);
            return links;
        }

        public List<GraphNode> GetNodes(string path)
        {
            string csv = ReadCsvString(path);
            string json = ParseCsvToJson(csv);
            List<GraphNode> nodes = JsonConvert.DeserializeObject<List<GraphNode>>(json);
            return nodes;
        }

        private string ParseCsvToJson(string csv)
        {
            JArray json = new JArray();
            List<string> csvLines = csv.Split(CsvLineSeparator).ToList();
            List<string> fieldNames = csvLines.First().Split(CsvColumnSeparator).ToList();

            foreach(string line in csvLines.Skip(1).Where(r => string.IsNullOrEmpty(r) == false))
            {
                List<string> lineColumns = line.Split(CsvColumnSeparator).ToList();
                JObject element = new JObject();
                for(int column = 0; column < lineColumns.Count(); column++)
                    element[fieldNames[column].ToString()] = lineColumns[column];
                json.Add(element);
            }

            return json.ToString();
        }

        private string ReadCsvString(string path)
        {
            string csv = File.ReadAllText(path);
            return csv;
        } 
    }
}
