using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Users
{
    [DataContract]
    public class UserData
    {
        #region Properties

        [DataMember]
        [Required]
        [EmailAddress]
        public string Login { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion
    }
}
