using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Users
{
    /// <summary>
    /// User
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// User
        /// </summary>
        public User()
        {
            Modified = DateTime.Now;
            Created = DateTime.Now;
            Status = UserStatus.Unlocked;
        }

        /// <summary>
        /// User
        /// </summary>
        /// <param name="userData"></param>
        public User(UserData userData)
        {
            UserData = userData;
            Modified = DateTime.Now;
            Created = DateTime.Now;
            Status = UserStatus.Unlocked;
        }

        /// <summary>
        /// UserData
        /// </summary>
        [DataMember]
        public UserData UserData { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public UserStatus Status { get; set; }

        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }
    }
}
