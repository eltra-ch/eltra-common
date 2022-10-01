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
        /// Content
        /// </summary>
        public static string RawData => "KEEPALIVE";

        /// <summary>
        /// WsMessageKeepAlive
        /// </summary>
        public WsMessageKeepAlive()
        {
            Data = RawData;
            TypeName = typeof(WsMessageAck).FullName;
        }
    }
}
