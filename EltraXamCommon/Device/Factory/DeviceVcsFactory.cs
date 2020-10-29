using EltraCommon.Contracts.Devices;
using EltraConnector.Agent;

namespace EltraXamCommon.Device.Factory
{
    public interface IDeviceVcsFactory
    {
        VirtualCommandSet CreateVcs(AgentConnector agent, EltraDevice device);
    }
}
