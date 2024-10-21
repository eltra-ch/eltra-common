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
using System.Text.Json.Nodes;

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
        /// ToJson
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static string ToJson(this ChannelBase channel)
        {
            return JsonSerializer.Serialize(channel);
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

            const string className = "JsonExtensions";
            const string method = "TryDeserializeObject";

            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    dynamic jsonNodes = JsonNode.Parse(json);

                    if (jsonNodes != null)
                    {
                        string classDiscriminator = GetClassIdentification<T>();

                        if (!string.IsNullOrEmpty(classDiscriminator))
                        {
                            string jsonDiscriminator = (string)jsonNodes["Discriminator"];

                            if (classDiscriminator == jsonDiscriminator)
                            {
                                result = JsonSerializer.Deserialize<T>(json);
                            }
                        }
                        else
                        {
                            MsgLogger.WriteError($"{className} - {method}", $"json, doesn't contain class identification");
                        }
                    }
                    else
                    {
                        MsgLogger.WriteError($"{className} - {method}", $"json parsing failed");
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{className} - {method}", e);
            }

            return result;
        }

        private static string GetClassIdentification<T>()
        {
            string result = string.Empty;
            
            var instance = (T)Activator.CreateInstance(typeof(T));
            var type = instance?.GetType();
            var headerProperty = type?.GetProperty("Discriminator");
            var headerValue = headerProperty?.GetValue(instance);

            if (headerValue != null)
            {
                result = Convert.ToString(headerValue);
            }

            return result;
        }
    }
}
