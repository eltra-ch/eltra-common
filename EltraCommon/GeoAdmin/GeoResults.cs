using System.Collections.Generic;
using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoResults
    {
        [DataMember]
        public List<GeoResult> Results { get; set; }
    }
}
