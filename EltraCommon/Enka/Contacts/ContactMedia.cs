using EltraCommon.Enka.Medias;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Contacts
{
    [DataContract]
    public class ContactMedia : BaseMedia
    {
        [DataMember]
        public ContactMediaType Type { get; set; }
    }
}
