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

            Start = DateTime.Now;
            End = Start + TimeSpan.FromHours(DefaultTimeoutInHours);

            Protocol = "json_v2";
        }

        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public OrderStatus Status { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Protocol { get; set; }
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
    }
}
