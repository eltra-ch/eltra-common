using EltraCommon.Enka.Medias;
using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.Enka.Orders
{
    [DataContract]
    public class OrderMedia : BaseMedia
    {
        [DataMember]
        public OrderMediaType Type { get; set; }
    }
}
