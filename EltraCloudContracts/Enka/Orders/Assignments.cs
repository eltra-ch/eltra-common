﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Orders
{
    [DataContract]
    public class Assignments
    {
        #region Private fields

        private List<Assignment> _entries;

        #endregion

        #region Constructors

        public Assignments()
        {
            MaxCount = 4;
        }

        #endregion

        #region Properties

        [DataMember]
        public int MaxCount { get; set; }
        [DataMember]
        public List<Assignment> Entries => _entries ?? (_entries = new List<Assignment>());

        #endregion

        #region Methods

        public bool AddEntry(Assignment assignment)
        {
            bool result = false;

            if (Entries.Count < MaxCount)
            {
                Entries.Add(assignment);
                result = true;
            }

            return result;
        }

        public void ClearEntries()
        {
            Entries.Clear();
        }

        #endregion
    }
}