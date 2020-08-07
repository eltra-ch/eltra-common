using System.Runtime.Serialization;

#pragma warning disable 1591

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
