using System.Runtime.Serialization;

#pragma warning disable 1591

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
