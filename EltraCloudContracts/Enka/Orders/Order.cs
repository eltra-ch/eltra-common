using System;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Orders
{
    [DataContract]
    public class Order
    {
        public Order()
        {
            const double DefaultTimeoutInHours = 4;

            Timeout = (int)TimeSpan.FromHours(DefaultTimeoutInHours).TotalSeconds;
        }

        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public OrderType Type { get; set; }
        [DataMember]
        public OrderStatus Status { get; set; }
        [DataMember]
        public string Notice { get; set; }
        [DataMember]
        public int Timeout { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
    }
}
