using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoStreetResults
    {
        [DataMember]
        public List<GeoStreetResult> Results { get; set; }
    }
}
