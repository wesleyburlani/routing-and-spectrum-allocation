using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation
{
    interface IRoutingAndSpectrumAllocation
    {
        Task Start(string readNodesPath, string readLinksPath, int numberOfLinkChannels);
    }
}
