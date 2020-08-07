using System;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Medias
{
    /// <summary>
    /// Media
    /// </summary>
    [DataContract]
    public class Media
    {
        /// <summary>
        /// Type
        /// </summary>
        [DataMember]
        public MediaType Type { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        [DataMember]
        public string Content { get; set; }
        /// <summary>
        /// HashCode
        /// </summary>
        [DataMember]
        public string HashCode { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }
        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }

        /// <summary>
        /// GetContentLength
        /// </summary>
        /// <returns></returns>
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
