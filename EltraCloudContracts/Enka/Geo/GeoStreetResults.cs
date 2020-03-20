using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp5
{
    [DataContract]
    public class GeoStreetResults
    {
        [DataMember]
        public List<GeoStreetResult> Results { get; set; }
    }
}
