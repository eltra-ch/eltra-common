using System;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Medias
{
    [DataContract]
    public class Media
    {
        [DataMember]
        public MediaType Type { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public string HashCode { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }

        public int GetContentLength()
        {
            int result = 0;

            if (!string.IsNullOrEmpty(Content))
            {
                result = Content.Length;
            }

            return result;
        }
    }
}
