using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Ws
{
    /// <summary>
    /// WsMessageAck
    /// </summary>
    [DataContract]
    public class WsMessageAck : WsMessage
    {
        /// <summary>
        /// Content
        /// </summary>
        public static string RawData => "ACK";

        /// <summary>
        /// WsMessageAck
        /// </summary>
        public WsMessageAck()
        {
            Data = RawData;
            TypeName = typeof(WsMessageAck).FullName;
        }
    }
}
