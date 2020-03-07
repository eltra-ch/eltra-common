using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Contracts.Users
{
    [DataContract]
    public class UserAuthData
    {
        #region Properties

        [DataMember]
        [Required]
        [EmailAddress]
        public string Login { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion
    }
}
