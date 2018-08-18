using System;
using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.InfoLoggers
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
