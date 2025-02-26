﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using EltraCommon.Contracts.Channels.Events;
using EltraCommon.Contracts.Devices;
using EltraCommon.Contracts.Location;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// Channel
    /// </summary>
    [DataContract]
    public class Channel : ChannelBase
    {
        #region Private fields

        private const uint DefaultUpdateInterval = 30;
        private const string DefaultDiscriminator = "Channel";

        private string _userName;
        private GeoLocation _location;
        private List<EltraDevice> _devices;
        private ChannelStatus _status;

        #endregion

        #region Constructors

        /// <summary>
        /// Channel
        /// </summary>
        public Channel()
        {
            Discriminator = DefaultDiscriminator;

            Modified = DateTime.Now.ToUniversalTime();
            Created = DateTime.Now.ToUniversalTime();

            _status = ChannelStatus.Offline;

            Timeout = uint.MaxValue;
            UpdateInterval = DefaultUpdateInterval;
        }

        /// <summary>
        /// Channel
        /// </summary>
        /// <param name="channelBase"></param>
        public Channel(ChannelBase channelBase)
        {
            Discriminator = DefaultDiscriminator;

            Modified = DateTime.Now.ToUniversalTime();
            Created = DateTime.Now.ToUniversalTime();
            Status = ChannelStatus.Offline;

            Id = channelBase.Id;
            Timeout = channelBase.Timeout;
            LocalHost = channelBase.LocalHost;

            UpdateInterval = DefaultUpdateInterval;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event fired on channel status change
        /// </summary>
        public event EventHandler<ChannelStatusChangedEventArgs> StatusChanged;

        private void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, new ChannelStatusChangedEventArgs() { Status = Status });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Channel User Name
        /// </summary>
        [DataMember]
        public string UserName
        {
            get => _userName ?? (_userName = string.Empty);
            set => _userName = value;
        }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public ChannelStatus Status 
        { 
            get => _status;
            set 
            {
                if (_status != value)
                {
                    _status = value;

                    OnStatusChanged();
                }
            } 
        }

        /// <summary>
        /// Location
        /// </summary>
        [DataMember]
        public GeoLocation Location
        {
            get => _location ?? (_location = new GeoLocation());
            set => _location = value;
        }

        /// <summary>
        /// Update interval
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public uint UpdateInterval { get; set; }

        /// <summary>
        /// Devices
        /// </summary>
        [DataMember]
        public List<EltraDevice> Devices 
        { 
            get => _devices ?? (_devices = new List<EltraDevice>());
            set => _devices = value;
        }

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
