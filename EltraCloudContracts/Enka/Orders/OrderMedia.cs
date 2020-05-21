using EltraCloudContracts.Enka.Medias;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Orders
{
    [DataContract]
    public class OrderMedia : BaseMedia
    {
        [DataMember]
        public OrderMediaType Type { get; set; }
    }
}
