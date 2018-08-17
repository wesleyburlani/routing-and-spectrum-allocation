using RoutingAndSpectrumAllocation.Graphs;

namespace RoutingAndSpectrumAllocation.InputReaders
{
    public interface IInputParser
    {
        Graph GetGraphFromReader(IInputReader inputReader);
    }
}
