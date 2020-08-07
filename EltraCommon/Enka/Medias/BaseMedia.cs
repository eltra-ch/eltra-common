using System;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Medias
{
    /// <summary>
    /// BaseMedia
    /// </summary>
    [DataContract]
    public class BaseMedia
    {
        /// <summary>
        /// Uuid
        /// </summary>
        [DataMember]
        public string Uuid { get; set; }
        /// <summary>
        /// Media
        /// </summary>
        [DataMember]
        public Media Media { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public MediaStatus Status { get; set; }
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
    }
}