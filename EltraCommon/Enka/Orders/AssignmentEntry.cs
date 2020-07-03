using System.Runtime.Serialization;

namespace EltraCommon.Enka.Orders
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
