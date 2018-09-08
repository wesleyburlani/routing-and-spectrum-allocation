using Newtonsoft.Json;

namespace RoutingAndSpectrumAllocation.Demands
{
    public class Demand
    {
        public Demand(int id, string nodeIdFrom, string nodeIdTo, int slots)
        {
            Id = id;
            NodeIdFrom = nodeIdFrom;
            NodeIdTo = nodeIdTo;
            Slots = slots;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("from")]
        public string NodeIdFrom { get; set; }

        [JsonProperty("to")]
        public string NodeIdTo { get; set; }

        [JsonProperty("slots")]
        public int Slots { get; set; }
    }
}
