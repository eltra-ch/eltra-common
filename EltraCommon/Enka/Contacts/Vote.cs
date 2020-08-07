using System;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Contacts
{
    /// <summary>
    /// Vote
    /// </summary>
    [DataContract]
    public class Vote
    {
        private Rating _rating;

        /// <summary>
        /// ContactUuid
        /// </summary>
        [DataMember]
        public string ContactUuid { get; set; }
        /// <summary>
        /// OrderUuid
        /// </summary>
        [DataMember]
        public string OrderUuid { get; set; }
        /// <summary>
        /// Rating
        /// </summary>
        [DataMember]
        public Rating Rating 
        { 
            get => _rating ?? (_rating = new Rating()); 
            set { _rating = value; } 
        }
        /// <summary>
        /// Comment
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
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
