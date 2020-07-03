using EltraCommon.Enka.Medias;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Orders
{
    [DataContract]
    public class OrderMedia : BaseMedia
    {
        [DataMember]
        public OrderMediaType Type { get; set; }
    }
}
