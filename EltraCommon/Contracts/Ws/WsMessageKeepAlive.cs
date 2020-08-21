using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Ws
{
    /// <summary>
    /// WsMessageKeepAlive
    /// </summary>
    [DataContract]
    public class WsMessageKeepAlive : WsMessage
    {
        /// <summary>
        /// WsMessageKeepAlive
        /// </summary>
        public WsMessageKeepAlive()
        {
            Data = "KEEPALIVE";
            TypeName = typeof(WsMessageAck).FullName;
        }
    }
}
