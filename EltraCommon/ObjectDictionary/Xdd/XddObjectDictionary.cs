using EltraCommon.Contracts.Devices;
using EltraCommon.ObjectDictionary.Common;

namespace EltraCommon.ObjectDictionary.Xdd
{
    /// <summary>
    /// XddObjectDictionary
    /// </summary>
    public class XddObjectDictionary : DeviceObjectDictionary
    {
        /// <summary>
        /// XddObjectDictionary
        /// </summary>
        /// <param name="device"></param>
        public XddObjectDictionary(EltraDevice device)
            : base(device)
        {
        }
    }
}
