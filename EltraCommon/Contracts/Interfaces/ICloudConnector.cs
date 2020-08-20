using EltraCommon.Contracts.CommandSets;
using EltraCommon.Contracts.Devices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EltraCommon.Contracts.Interfaces
{
    /// <summary>
    /// IConnector
    /// </summary>
    public interface ICloudConnector
    {
        /// <summary>
        /// Get device commands.
        /// </summary>
        /// <param name="device">device instance</param>
        /// <returns>List of DeviceCommand</returns>
        Task<List<DeviceCommand>> GetDeviceCommands(EltraDevice device);
        /// <summary>
        /// Get device command.
        /// </summary>
        /// <param name="device">device instance</param>
        /// <param name="commandName">Command name.</param>
        /// <returns></returns>
        Task<DeviceCommand> GetDeviceCommand(EltraDevice device, string commandName);
        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="command">Command instance</param>
        /// <returns></returns>
        Task<DeviceCommand> ExecuteCommand(DeviceCommand command);
    }
}
