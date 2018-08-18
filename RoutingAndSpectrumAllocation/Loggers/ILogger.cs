using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.Loggers
{
    public interface ILogger
    {
        Task WriteLog(string Id, object obj, string subFolder="");
    }
}
