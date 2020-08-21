using System;

namespace EltraCommon.Contracts.Channels.Events
{
    /// <summary>
    /// ChannelStatusChangedEventArgs
    /// </summary>
    public class ChannelStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Channel status.
        /// </summary>
        public ChannelStatus Status { get; set; }
    }
}
