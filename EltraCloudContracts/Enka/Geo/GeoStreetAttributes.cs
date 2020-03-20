using System.Runtime.Serialization;

namespace ConsoleApp5
{
    [DataContract]
    public class GeoStreetAttributes
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}
