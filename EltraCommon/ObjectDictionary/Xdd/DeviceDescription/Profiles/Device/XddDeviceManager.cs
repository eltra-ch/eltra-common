using System.Xml;

using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Device.UserLevels;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Device
{
    public class XddDeviceManager
    {
        #region Private fields

        private XddUserLevelList _userLevelList;

        #endregion

        #region Properties

        public XddUserLevelList UserLevelList => _userLevelList ?? (_userLevelList = new XddUserLevelList(this));

        #endregion

        #region Methods

        public virtual bool Parse(XmlNode node)
        {
            bool result = true;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "userLevelList" && !UserLevelList.Parse(childNode))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public XddUserLevel FindReference(string uniqueId)
        {
            XddUserLevel result = null;

            foreach (var userLevel in UserLevelList.UserLevels)
            {
                if (userLevel.UniqueId == uniqueId)
                {
                    result = userLevel;
                    break;
                }
            }

            return result;
        }

        public virtual void ResolveParameterReferences(XddParameterList parameterList)
        {            
        }

        #endregion
    }
}
