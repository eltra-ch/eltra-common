using System;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Regional
{
    [DataContract]
    public class PostalCodeInfo
    {
        [DataMember]
        public Region Region { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
    }
}
