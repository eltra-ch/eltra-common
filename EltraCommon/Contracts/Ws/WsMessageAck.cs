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
        /// WsMessageAck
        /// </summary>
        public WsMessageAck()
        {
            Data = "ACK";
            TypeName = typeof(WsMessageAck).FullName;
        }
    }
}
