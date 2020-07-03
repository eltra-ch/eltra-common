using System.Runtime.Serialization;

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoStreetAttributes
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "plzo")]
        public string PostalCode { get; set; }
        [DataMember(Name = "gdename")]
        public string Municipality { get; set; }
        [DataMember(Name = "official")]
        public string Official { get; set; }
        [DataMember(Name = "modified")]
        public string Modified { get; set; }
        [DataMember(Name = "label")]
        public string Label { get; set; }
        [DataMember(Name = "gdenr")]
        public string MunicipalityNumber { get; set; }
        [DataMember(Name = "esid")]
        public string Esid { get; set; }
        [DataMember(Name = "validated")]
        public string Validated { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
