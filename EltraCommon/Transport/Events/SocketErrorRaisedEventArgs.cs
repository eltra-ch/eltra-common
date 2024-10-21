using System;
using System.Net.Sockets;

namespace EltraCommon.Transport.Events
{
    /// <summary>
    /// SocketErrorChangedEventArgs
    /// </summary>
    public class SocketErrorRaisedEventArgs : EventArgs
    {
        /// <summary>
        /// SocketError
        /// </summary>
        public SocketError SocketError { get; set; }
    }
}
