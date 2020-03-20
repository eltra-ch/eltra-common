using System.Runtime.Serialization;

namespace ConsoleApp5
{
    [DataContract]
    public class GeoStreetResult
    {
        [DataMember(Name= "featureId")] 
        public string FeatureId { get; set; }
        [DataMember(Name= "attributes")]
        public GeoStreetAttributes Attrs { get; set; }
    }
}
