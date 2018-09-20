using System;
namespace RoutingAndSpectrumAllocation.ModulationFormarts
{
    [Serializable]
    public class F64QAM : IModulationFormat
    {
        double IModulationFormat.GetCapacityInGbps()
        {
            return 75;
        }

        double IModulationFormat.GetReachInKm()
        {
            return 125;
        }
    }
}
