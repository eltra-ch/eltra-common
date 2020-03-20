using System.Runtime.Serialization;

namespace EltraCloudContracts.GeoAdmin
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
