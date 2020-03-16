using System.Runtime.Serialization;
using EltraCloudContracts.Enka.Contacts;

namespace EltraCloudContracts.Enka.Orders
{
    [DataContract]
    public class OrderInfo
    {
        [DataMember]
        public Order Order { get; set; }
        [DataMember]
        public Contact CreatedBy { get; set; }
    }
}
