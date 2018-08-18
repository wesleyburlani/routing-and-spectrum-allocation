using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.Loggers
{
    public interface IInfoLogger
    {
        Task LogInformation(string payload);
    }
}
