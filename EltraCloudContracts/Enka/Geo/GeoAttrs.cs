using System.Runtime.Serialization;

namespace ConsoleApp5
{
    [DataContract(Name = "attrs")]
    public class GeoAttrs
    {
        [DataMember]
        public string Origin { get; set; }
        [DataMember(Name = "geom_quadindex")]
        public string GeomQuadIndex { get; set; }
        [DataMember(Name = "zoomlevel")]
        public string ZoomLevel { get; set; }
        [DataMember(Name = "featureId")]
        public string FeatureId { get; set; }
        [DataMember(Name = "lon")]
        public string Longitude { get; set; }
        [DataMember(Name = "detail")]
        public string Detail { get; set; }
        [DataMember(Name = "rank")]
        public string Rank { get; set; }
        [DataMember(Name = "geom_st_box2d")]
        public string GeomStBox2d { get; set; }
        [DataMember(Name = "lat")]
        public string Latitude { get; set; }
        [DataMember(Name = "num")]
        public int Num { get; set; }
    }
}
