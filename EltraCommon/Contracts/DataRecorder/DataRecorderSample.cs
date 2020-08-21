using System.Runtime.Serialization;

namespace EltraCommon.Contracts.DataRecorder
{
    /// <summary>
    /// DataRecorderSample
    /// </summary>
    [DataContract]
    public class DataRecorderSample
    {
        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public int Value { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [DataMember]
        public uint Timestamp { get; set; }
    }
}
