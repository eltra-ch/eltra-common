using EltraCommon.Enka.Contacts;
using System;
using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.Enka.Orders
{
    [DataContract]
    public class Assignment
    {
        public Assignment()
        {
        }

        public Assignment(Order order)
        {
            Order = order;
        }

        [DataMember]
        public Order Order { get; set; }
        [DataMember]
        public Contact CreatedBy { get; set; }
        [DataMember]
        public AssignmentStatus Status { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
    }
}
