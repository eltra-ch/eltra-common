using EltraCommon.Enka.Medias;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Contacts
{
    /// <summary>
    /// ContactMedia
    /// </summary>
    [DataContract]
    public class ContactMedia : BaseMedia
    {
        /// <summary>
        /// Type
        /// </summary>
        [DataMember]
        public ContactMediaType Type { get; set; }
    }
}
