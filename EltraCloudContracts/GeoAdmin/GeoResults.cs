using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCloudContracts.GeoAdmin
{
    [DataContract]
    public class GeoResults
    {
        [DataMember]
        public List<GeoResult> Results { get; set; }
    }
}
