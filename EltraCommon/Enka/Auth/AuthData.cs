using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Auth
{
    /// <summary>
    /// AuthData
    /// </summary>
    [DataContract]
    public class AuthData
    {
        #region Properties

        /// <summary>
        /// Login
        /// </summary>
        [DataMember]
        [Required]
        [EmailAddress]
        public string Login { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion
    }
}
