using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Ws
{
    [DataContract]
    public class WsMessageAck : WsMessage
    {
        public WsMessageAck()
        {
            Data = "ACK";
            TypeName = typeof(WsMessageAck).FullName;
        }
    }
}
