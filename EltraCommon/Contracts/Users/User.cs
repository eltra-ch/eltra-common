using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Users
{
    [DataContract]
    public class User
    {
        public User()
        {
            Modified = DateTime.Now;
            Created = DateTime.Now;
            Status = UserStatus.Unlocked;
        }

        public User(UserData userData)
        {
            UserData = userData;
            Modified = DateTime.Now;
            Created = DateTime.Now;
            Status = UserStatus.Unlocked;
        }

        [DataMember]
        public UserData UserData { get; set; }

        [DataMember]
        public UserStatus Status { get; set; }

        [DataMember]
        public DateTime Modified { get; set; }

        [DataMember]
        public DateTime Created { get; set; }
    }
}
