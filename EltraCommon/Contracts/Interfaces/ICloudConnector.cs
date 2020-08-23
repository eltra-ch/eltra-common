using EltraCommon.Contracts.CommandSets;
using EltraCommon.Contracts.Devices;
using EltraCommon.Contracts.History;
using EltraCommon.Contracts.Parameters;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.DeviceDescription;
using System;
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

        /// <summary>
        /// Write parameter.
        /// </summary>
        /// <param name="device">Device</param>
        /// <param name="parameter">Parameter</param>
        /// <returns></returns>
        Task<bool> WriteParameter(EltraDevice device, Parameter parameter);

        /// <summary>
        /// Get parameter value history.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="index"></param>
        /// <param name="subIndex"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        Task<List<ParameterValue>> GetParameterValueHistory(EltraDevice device, ushort index, byte subIndex, DateTime from, DateTime to);

        /// <summary>
        /// Get parameter value history statistics
        /// </summary>
        /// <param name="device"></param>
        /// <param name="uniqueId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        Task<ParameterValueHistoryStatistics> GetParameterValueHistoryStatistics(EltraDevice device, string uniqueId, DateTime from, DateTime to);

        /// <summary>
        /// Get parameter value
        /// </summary>
        /// <param name="device">Device</param>
        /// <param name="index">Index</param>
        /// <param name="subIndex">SubIndex</param>
        /// <returns></returns>
        Task<ParameterValue> GetParameterValue(EltraDevice device, ushort index, byte subIndex);

        /// <summary>
        /// Get parameter by index, subindex.
        /// </summary>
        /// <param name="device">Device</param>
        /// <param name="index">Index</param>
        /// <param name="subIndex">SubIndex</param>
        /// <returns></returns>
        Task<Parameter> GetParameter(EltraDevice device, ushort index, byte subIndex);

        /// <summary>
        /// Register parameter update.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="uniqueId"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        bool RegisterParameterUpdate(EltraDevice device, string uniqueId, ParameterUpdatePriority priority = ParameterUpdatePriority.Low);

        /// <summary>
        /// Unregister parameter update.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="uniqueId"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        bool UnregisterParameterUpdate(EltraDevice device, string uniqueId, ParameterUpdatePriority priority = ParameterUpdatePriority.Low);

        /// <summary>
        /// Download Device Description
        /// </summary>
        /// <param name="deviceVersion"></param>
        /// <returns></returns>
        Task<DeviceDescriptionPayload> DownloadDeviceDescription(DeviceVersion deviceVersion);
    }
}
