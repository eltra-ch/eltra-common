using System.Runtime.Serialization;

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoStreetInfo
    {
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
    }
}
