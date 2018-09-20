using System;
namespace RoutingAndSpectrumAllocation.ModulationFormarts
{
    [Serializable]
    public class F32QAM : IModulationFormat
    {
        public double GetCapacityInGbps()
        {
            return 62.5;
        }

        public double GetReachInKm()
        {
            return 250;
        }
    }
}
