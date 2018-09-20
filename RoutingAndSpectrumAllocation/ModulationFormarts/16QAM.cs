using System;
namespace RoutingAndSpectrumAllocation.ModulationFormarts
{
    [Serializable]
    public class F16QAM : IModulationFormat
    {
        public double GetCapacityInGbps()
        {
            return 50;
        }

        public double GetReachInKm()
        {
            return 500;
        }
    }
}
