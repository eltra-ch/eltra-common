using System;

using EltraCommon.Contracts.CommandSets;
using EltraCommon.Contracts.Channels;
using EltraCommon.Contracts.Devices;
using System.Text.Json;
using EltraCommon.Logger;
using EltraCommon.Contracts.Users;
using EltraCommon.ObjectDictionary.DeviceDescription;
using EltraCommon.Contracts.ToolSet;
using EltraCommon.Contracts.Parameters;

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
            return JsonSerializer.Serialize(device);
        }

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="device"></param>
        /// <param name="json"></param>
        public static void FromJson(this EltraDevice device, string json)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            device = TryDeserializeObject<EltraDevice>(json);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static string ToJson(this ChannelBase channel)
        {
            return JsonSerializer.Serialize(channel);
        }

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="session"></param>
        /// <param name="json"></param>
        public static void FromJson(this Channel session, string json)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            session = TryDeserializeObject<Channel>(json);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="sessionDevices"></param>
        /// <returns></returns>
        public static string ToJson(this EltraDeviceSet sessionDevices)
        {
            return JsonSerializer.Serialize(sessionDevices);
        }

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="sessionDevices"></param>
        /// <param name="json"></param>
        public static void FromJson(this EltraDeviceSet sessionDevices, string json)
        {
            if (sessionDevices == null) throw new ArgumentNullException(nameof(sessionDevices));

            sessionDevices = TryDeserializeObject<EltraDeviceSet>(json);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="execCommand"></param>
        /// <returns></returns>
        public static string ToJson(this ExecuteCommand execCommand)
        {
            return JsonSerializer.Serialize(execCommand);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string ToJson(this UserIdentity identity)
        {
            return JsonSerializer.Serialize(identity);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string ToJson(this DeviceDescriptionPayload payload)
        {
            return JsonSerializer.Serialize(payload);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string ToJson(this DeviceToolPayload payload)
        {
            return JsonSerializer.Serialize(payload);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static string ToJson(this ParameterUpdate update)
        {
            return JsonSerializer.Serialize(update);
        }

        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static string ToJson(this ParameterValueUpdate update)
        {
            return JsonSerializer.Serialize(update);
        }

        /// <summary>
        /// TryDeserializeObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T TryDeserializeObject<T>(this string json)
        {
            T result = default;

            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    string headerIdentification = string.Empty;

                    headerIdentification = GetClassIdentification<T>();

                    if (!string.IsNullOrEmpty(headerIdentification))
                    {
                        if (json.Contains($"\"Header\":\"{headerIdentification}\""))
                        {
                            result = JsonSerializer.Deserialize<T>(json);
                        }
                        else
                        {
                            MsgLogger.WriteError("TryDeserializeObject", $"json, doesn't contain header {headerIdentification}");
                        }
                    }
                    else
                    {
                        MsgLogger.WriteError("TryDeserializeObject", $"json, doesn't contain class identification");
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"JsonExtensions - TryDeserializeObject", e);
            }

            return result;
        }

        private static string GetClassIdentification<T>()
        {
            string result = string.Empty;
            
            var instance = (T)Activator.CreateInstance(typeof(T));
            var type = instance?.GetType();
            var headerProperty = type?.GetProperty("Header");
            var headerValue = headerProperty?.GetValue(instance);

            if (headerValue != null)
            {
                result = Convert.ToString(headerValue);
            }

            return result;
        }
    }
}
