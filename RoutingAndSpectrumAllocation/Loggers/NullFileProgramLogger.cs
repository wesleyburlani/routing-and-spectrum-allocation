using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.Loggers
{
    class NullFileProgramLogger : IProgramLogger
    {
        public Task LogInformation(string payload)
        {
            return Task.CompletedTask;
        }
    }
}
