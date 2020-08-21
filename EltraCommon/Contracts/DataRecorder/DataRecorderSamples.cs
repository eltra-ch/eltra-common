using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.DataRecorder
{
    /// <summary>
    /// DataRecorderSamples
    /// </summary>
    [DataContract]
    public class DataRecorderSamples
    {
        private List<DataRecorderSample> _samples;

        /// <summary>
        /// ChannelNumber
        /// </summary>
        [DataMember]
        public byte ChannelNumber { get; set; }

        /// <summary>
        /// Samples
        /// </summary>
        [DataMember]
        public List<DataRecorderSample> Samples { get => _samples ?? (_samples = new List<DataRecorderSample>()); }

        /// <summary>
        /// SamplingPeriod
        /// </summary>
        [DataMember]
        public ushort SamplingPeriod { get; set; }

        /// <summary>
        /// LastTimestamp
        /// </summary>
        [DataMember]
        public uint LastTimestamp { get; set; }

        /// <summary>
        /// CreateSamples
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool CreateSamples(byte[] data, ushort size)
        {
            bool result = false;

            if (data.Length >= sizeof(int) * size)
            {
                uint samplingPeriodInUs = (uint)10 * SamplingPeriod;
                uint timestamp = (uint)System.Math.Abs(LastTimestamp - (double)(size * samplingPeriodInUs));
                
                for (int i = 0; i < size; i++)
                {
                    var sample = new DataRecorderSample {Timestamp = timestamp};

                    sample.Value = BitConverter.ToInt32(data.Skip(i*sizeof(int)).Take(sizeof(int)).ToArray(),0);

                    Samples.Add(sample);

                    timestamp += samplingPeriodInUs;
                }

                result = true;
            }

            return result;
        }
    }
}
