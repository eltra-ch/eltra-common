﻿using System;
using System.Runtime.Serialization;
using EltraCommon.Contracts.Location;
using EltraCommon.Contracts.Users;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// Channel
    /// </summary>
    [DataContract]
    public class Channel
    {
        #region Private fields

        private const uint DefaultUpdateInterval = 30;

        private User _user;
        private IpLocation _location;

        #endregion

        #region Constructors

        /// <summary>
        /// Channel
        /// </summary>
        public Channel()
        {
            Modified = DateTime.Now;
            Created = DateTime.Now;
            Status = ChannelStatus.Offline;
            Timeout = uint.MaxValue;
            UpdateInterval = DefaultUpdateInterval;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [DataMember]
        public User User
        {
            get => _user ?? (_user = new User());
            set => _user = value;
        }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public ChannelStatus Status { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [DataMember]
        public IpLocation IpLocation
        {
            get => _location ?? (_location = new IpLocation());
            set => _location = value;
        }

        /// <summary>
        /// Timeout
        /// </summary>
        [DataMember]
        public uint Timeout { get; set; }

        /// <summary>
        /// Update interval
        /// </summary>
        [IgnoreDataMember]
        public uint UpdateInterval { get; set; }

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

        #endregion
    }
}
