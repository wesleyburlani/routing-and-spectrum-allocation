using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RoutingAndSpectrumAllocation.Demands
{
    public class DemandReader
    {
        public DemandReader()
        {
        }

        public List<Demand> GetDemands()
        {
            return JsonConvert.DeserializeObject<List<Demand>>(File.ReadAllText("../Output/demands.json"));
        }
    }
}
