using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoStreetResult
    {
        [DataMember(Name= "featureId")] 
        public string FeatureId { get; set; }
        [DataMember(Name= "attributes")]
        public GeoStreetAttributes Attrs { get; set; }
        [DataMember(Name = "layerBodId")]
        public string LayerBodyId { get; set; }
        [DataMember(Name = "layerName")]
        public string LayerName { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
