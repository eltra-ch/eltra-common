using System;

using EltraCommon.Contracts.CommandSets;
using EltraCommon.Contracts.Channels;
using EltraCommon.Contracts.Devices;
using Newtonsoft.Json;

namespace EltraCommon.Extensions
{
    /// <summary>
    /// JsonExtensions
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static string ToJson(this EltraDevice device)
        {
            return JsonConvert.SerializeObject(device);
        }

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="device"></param>
        /// <param name="json"></param>
        public static void FromJson(this EltraDevice device, string json)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            device = JsonConvert.DeserializeObject<EltraDevice>(json);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static string ToJson(this ChannelBase channel)
        {
            return JsonConvert.SerializeObject(channel);
        }

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="session"></param>
        /// <param name="json"></param>
        public static void FromJson(this Channel session, string json)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            session = JsonConvert.DeserializeObject<Channel>(json);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="sessionDevices"></param>
        /// <returns></returns>
        public static string ToJson(this EltraDeviceSet sessionDevices)
        {
            return JsonConvert.SerializeObject(sessionDevices);
        }

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="sessionDevices"></param>
        /// <param name="json"></param>
        public static void FromJson(this EltraDeviceSet sessionDevices, string json)
        {
            if (sessionDevices == null) throw new ArgumentNullException(nameof(sessionDevices));

            sessionDevices = JsonConvert.DeserializeObject<EltraDeviceSet>(json);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="execCommand"></param>
        /// <returns></returns>
        public static string ToJson(this ExecuteCommand execCommand)
        {
            return JsonConvert.SerializeObject(execCommand);
        }
    }
}
