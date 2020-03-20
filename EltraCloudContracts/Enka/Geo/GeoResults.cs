using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp5
{
    [DataContract]
    public class GeoResults
    {
        [DataMember]
        public List<GeoResult> Results { get; set; }
    }
}
