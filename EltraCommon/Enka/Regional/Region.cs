using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.Enka.Regional
{
    [DataContract]
    public class Region
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName { get; set; }
    }
}
