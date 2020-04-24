using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Orders
{
    [DataContract]
    public class AssignmentEntry
    {
        [DataMember]
        public string OrderUuid { get; set; }
        [DataMember]
        public string CreatedByUuid { get; set; }
        [DataMember]
        public AssignmentStatus Status { get; set; }
    }
}
