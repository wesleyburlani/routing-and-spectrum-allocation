using System;
namespace RoutingAndSpectrumAllocation.ModulationFormarts
{
    [Serializable]
    public class BPKS : IModulationFormat
    {
        public double GetCapacityInGbps()
        {
            return 12.5;
        }

        public double GetReachInKm()
        {
            return 4000;
        }
    }
}
