using System;
using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.Loggers
{
    public class InfoLogger : IInfoLogger
    {
        public async Task LogInformation(string payload)
        {
           await Task.Factory.StartNew(() =>
           {
               Console.WriteLine(payload);
           });
        }
    }
}
