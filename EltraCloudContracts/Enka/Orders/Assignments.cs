using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Orders
{
    [DataContract]
    public class Assignments
    {
        private List<Assignment> _entries;

        [DataMember]
        public int MaxCount { get; set; }
        [DataMember]
        public List<Assignment> Entries => _entries ?? (_entries = new List<Assignment>());
    }
}
