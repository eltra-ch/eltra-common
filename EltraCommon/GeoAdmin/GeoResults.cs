using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoResults
    {
        [DataMember]
        public List<GeoResult> Results { get; set; }
    }
}
