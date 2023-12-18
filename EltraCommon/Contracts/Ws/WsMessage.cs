using EltraCommon.Contracts.Users;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Ws
{
    /// <summary>
    /// WsMessage
    /// </summary>
    [DataContract]
    public class WsMessage
    {
        /// <summary>
        /// WsMessage
        /// </summary>
        public WsMessage()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "WsMessage";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// User authorization data
        /// </summary>
        [DataMember]
        public UserIdentity Identity { get; set; }

        /// <summary>
        /// ChannelName
        /// </summary>
        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        /// TypeName
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        [DataMember]
        public string Data { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [DataMember]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Checksum
        /// </summary>
        [DataMember]
        public string Checksum { get; set; }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(WsMessage obj)
        {
            bool result = false;

            if(obj != null && obj.Data == Data && obj.TypeName == TypeName)
            {
                result = true;
            }

            return result;
        }
    }
}

