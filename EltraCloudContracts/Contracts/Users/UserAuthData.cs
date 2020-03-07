using System.Runtime.Serialization;

namespace EltraCloudContracts.Contracts.Users
{
    [DataContract]
    public class UserAuthData
    {
        #region Properties

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Password { get; set; }

        #endregion
    }
}
