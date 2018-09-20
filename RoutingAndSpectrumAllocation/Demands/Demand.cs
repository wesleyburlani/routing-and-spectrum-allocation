using Newtonsoft.Json;

namespace RoutingAndSpectrumAllocation.Demands
{
    public class Demand
    {
        public Demand(int id, string nodeIdFrom, string nodeIdTo, int slots, double demandInGbps)
        {
            Id = id;
            NodeIdFrom = nodeIdFrom;
            NodeIdTo = nodeIdTo;
            Slots = slots;
            DemandInGBps = demandInGbps;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("from")]
        public string NodeIdFrom { get; set; }

        [JsonProperty("to")]
        public string NodeIdTo { get; set; }

        [JsonProperty("demandInGbps")]
        public double DemandInGBps { get; set; }

        [JsonProperty("slots")]
        public int Slots { get; set; }
    }
}
