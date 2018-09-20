using System;
namespace RoutingAndSpectrumAllocation.ModulationFormarts
{
    [Serializable]
    public class F8QAM : IModulationFormat
    {
        public double GetCapacityInGbps()
        {
            return 37.5;
        }

        public double GetReachInKm()
        {
            return 1000;
        }
    }
}
