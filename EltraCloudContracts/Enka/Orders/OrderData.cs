using EltraCloudContracts.Enka.Contacts;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Orders
{
    [DataContract]
    public class OrderData
    {
        private Assignments _assignments;

        [DataMember]
        public Order Order { get; set; }
        [DataMember]
        public Contact CreatedBy { get; set; }
        [DataMember]
        public Assignments Assignments => _assignments ?? (_assignments = new Assignments());
    }
}
