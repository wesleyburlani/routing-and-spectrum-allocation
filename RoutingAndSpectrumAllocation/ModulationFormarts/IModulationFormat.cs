using System;
namespace RoutingAndSpectrumAllocation.ModulationFormarts
{
    public interface IModulationFormat
    {
        double GetCapacityInGbps();
        double GetReachInKm();

    }
}
