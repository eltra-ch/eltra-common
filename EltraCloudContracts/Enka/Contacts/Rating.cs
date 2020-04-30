using System;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Contacts
{
    [DataContract]
    public class Rating
    {
        [DataMember]
        public double MinValue { get => 0; }
        [DataMember]
        public double MaxValue { get => 5; }
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        
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
