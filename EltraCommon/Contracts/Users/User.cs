﻿using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
        /// <param name="identity"></param>
        public User(UserIdentity identity)
        {
            Identity = identity;

            Modified = DateTime.Now;
            Created = DateTime.Now;
            Status = UserStatus.Unlocked;
        }

        /// <summary>
        /// UniqueId
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public string UniqueId { get; set; }

        /// <summary>
        /// Identity
        /// </summary>
        [DataMember]
        public UserIdentity Identity { get; set; }

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
