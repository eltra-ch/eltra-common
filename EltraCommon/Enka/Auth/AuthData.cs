using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Auth
{
    [DataContract]
    public class AuthData
    {
        #region Properties

        [DataMember]
        [Required]
        [EmailAddress]
        public string Login { get; set; }

        [DataMember]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion
    }
}
