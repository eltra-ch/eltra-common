using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class SessionsDevices
    {
        private List<SessionDevices> _sessionDevices;

        [DataMember]
        public List<SessionDevices> SessionDevices
        {
            get => _sessionDevices ?? (_sessionDevices = new List<SessionDevices>());
        }

        public void Add(SessionDevices sessionDevices)
        {
            SessionDevices.Add(sessionDevices);
        }
    }
}
