using System;
namespace RoutingAndSpectrumAllocation.ModulationFormarts
{
    [Serializable]
    public class QPSK : IModulationFormat
    {
        public double GetCapacityInGbps()
        {
            return 25;
        }

        public double GetReachInKm()
        {
            return 2000;
        }
    }
}
