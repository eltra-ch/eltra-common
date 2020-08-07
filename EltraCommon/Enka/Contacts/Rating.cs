using System;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Contacts
{
    /// <summary>
    /// Rating
    /// </summary>
    [DataContract]
    public class Rating
    {
        /// <summary>
        /// MinValue
        /// </summary>
        [DataMember]
        public double MinValue { get => 0; }
        /// <summary>
        /// MaxValue
        /// </summary>
        [DataMember]
        public double MaxValue { get => 5; }
        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public double Value { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }
        /// <summary>
        /// InRange - optional
        /// </summary>
        [IgnoreDataMember]
        public bool InRange
        {
            get 
            {
                bool result = false;

                if (Value > MinValue && Value <= MaxValue)
                {
                    result = true;
                }

                return result;
            }
        }
    }
}
