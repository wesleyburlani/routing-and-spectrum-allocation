using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.Loggers
{
    public interface IStorageLogger
    {
        Task WriteLog(string Id, object obj, string subFolder="");
    }
}
