using System.Collections.Generic;
using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoStreetResults
    {
        [DataMember]
        public List<GeoStreetResult> Results { get; set; }
    }
}
