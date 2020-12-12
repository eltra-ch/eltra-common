using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Users
{
    /// <summary>
    /// UserIdentity
    /// </summary>
    [DataContract]
    public class UserIdentity
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
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        [DataMember]
        public string Role { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        [IgnoreDataMember]
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
            }
        }

        #endregion
    }
}
