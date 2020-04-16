using EltraCloudContracts.Enka.Contacts;
using System;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Orders
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
