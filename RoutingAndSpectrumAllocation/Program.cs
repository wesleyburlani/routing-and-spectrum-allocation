using Microsoft.Extensions.DependencyInjection;
using RoutingAndSpectrumAllocation.Loggers;
using RoutingAndSpectrumAllocation.InputReaders;

namespace RoutingAndSpectrumAllocation
{
    class Program
    {
        const char CsvLineSeparator = '\n';
        const char CsvColumnSeparator = ',';
        const string LogPath = @"Log";
        const string ReadNodesPath = @"Data\arnes_nodes.csv";
        const string ReadLinksPath = @"Data\arnes_links.csv";

        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IRoutingAndSpectrumAllocation, RoutingAndSpectrumAllocation>();
            serviceCollection.AddScoped<IInfoLogger, InfoLogger>();
            serviceCollection.AddScoped<IStorageLogger>(c => new JsonFileLogger(LogPath));
            serviceCollection.AddScoped<IGraphInputReader>(c => new CsvGraphInputReader(CsvLineSeparator, CsvColumnSeparator));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var routingAndSpectrumAllocation = serviceProvider.GetService<IRoutingAndSpectrumAllocation>();
            routingAndSpectrumAllocation.Start(ReadNodesPath, ReadLinksPath).Wait();
        }
    }
}
