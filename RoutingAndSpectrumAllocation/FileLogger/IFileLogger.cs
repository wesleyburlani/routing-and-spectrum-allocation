using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.FileLogger
{
    public interface IFileLogger
    {
        Task WriteLog(string Id, object obj, string subFolder="");
    }
}
