using Microsoft.Extensions.DependencyInjection;
using RoutingAndSpectrumAllocation.InputReaders;

namespace RoutingAndSpectrumAllocation
{
    class Program
    {
        const char CsvLineSeparator = '\n';
        const char CsvColumnSeparator = ',';

        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IRoutingAndSpectrumAllocation, RoutingAndSpectrumAllocation>();
            serviceCollection.AddScoped<IInputReader>(c => new CsvInputReader(CsvLineSeparator, CsvColumnSeparator));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var routingAndSpectrumAllocation = serviceProvider.GetService<IRoutingAndSpectrumAllocation>();
        }
    }
}
