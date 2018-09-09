using Microsoft.Extensions.DependencyInjection;
using RoutingAndSpectrumAllocation.Loggers;
using RoutingAndSpectrumAllocation.InputReaders;
using RoutingAndSpectrumAllocation.Graphs;
using RoutingAndSpectrumAllocation.RSA;

namespace RoutingAndSpectrumAllocation
{
    class Program
    {
        const char CsvLineSeparator = '\n';
        const char CsvColumnSeparator = ',';
        const string LogPath = @"Output";
        const string ReadNodesPath = @"Data/arnes_nodes.csv";
        const string ReadLinksPath = @"Data/arnes_links.csv";

        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IRoutingAndSpectrumAllocation, SharedProtectionRSA>();
            serviceCollection.AddScoped<IProgramLogger>(c => new FileProgramLogger(LogPath));
            //serviceCollection.AddScoped<IProgramLogger, NullFileProgramLogger>();
            serviceCollection.AddScoped<ILogger>(c => new JsonFileLogger(LogPath));
            serviceCollection.AddScoped<IRSATableFill, FirstFitRSATableFill>();
            serviceCollection.AddScoped<IGraphInputReader>(c => new CsvGraphInputReader(CsvLineSeparator, CsvColumnSeparator));
            serviceCollection.AddScoped<IPathSearcher, Dijkstra>();
            serviceCollection.AddScoped<IDisjointedPathPairSearcher, Suurballe>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var routingAndSpectrumAllocation = serviceProvider.GetService<IRoutingAndSpectrumAllocation>();
            routingAndSpectrumAllocation.Start(ReadNodesPath, ReadLinksPath, 8).Wait();
        }
    }
}
