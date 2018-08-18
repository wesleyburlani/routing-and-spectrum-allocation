using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.InfoLoggers
{
    public interface IInfoLogger
    {
        Task LogInformation(string payload);
    }
}
