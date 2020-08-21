using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Devices
{
    /// <summary>
    /// DeviceIdentification
    /// </summary>
    [DataContract]
    public class DeviceIdentification
    {
        #region Properties

        /// <summary>
        /// Device serial number
        /// </summary>
        [DataMember]
        public ulong SerialNumber { get; set; }

        /// <summary>
        /// Device name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Read
        /// </summary>
        /// <returns></returns>
        public virtual bool Read()
        {
            return false;
        }

        #endregion
    }
}
