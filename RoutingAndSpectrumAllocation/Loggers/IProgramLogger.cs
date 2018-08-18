using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.Loggers
{
    public interface IProgramLogger
    {
        Task LogInformation(string payload);
    }
}
