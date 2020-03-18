using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Regional
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
