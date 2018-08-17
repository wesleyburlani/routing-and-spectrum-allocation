using Newtonsoft.Json;

namespace RoutingAndSpectrumAllocation.Graphs
{
    public class GraphNode
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Lat")]
        public double Lat { get; set; }

        [JsonProperty("Long")]
        public double Long { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}
