using Newtonsoft.Json;

namespace RoutingAndSpectrumAllocation.Demands
{
    public class Demand
    {
        public Demand(string nodeIdFrom, string nodeIdTo, int slots)
        {
            NodeIdFrom = nodeIdFrom;
            NodeIdTo = nodeIdTo;
            Slots = slots;
        }

        [JsonProperty("from")]
        public string NodeIdFrom { get; set; }

        [JsonProperty("to")]
        public string NodeIdTo { get; set; }

        [JsonProperty("slots")]
        public int Slots { get; set; }
    }
}
