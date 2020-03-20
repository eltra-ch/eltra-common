using System.Runtime.Serialization;

namespace EltraCloudContracts.GeoAdmin
{
    [DataContract]
    public class GeoResult
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Weight { get; set; }
        [DataMember]
        public GeoAttrs Attrs { get; set; }
    }
}
