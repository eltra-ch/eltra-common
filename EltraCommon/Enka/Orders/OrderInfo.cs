﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using EltraCommon.Enka.Contacts;

namespace EltraCommon.Enka.Orders
{
    [DataContract]
    public class OrderInfo
    {
        [DataMember]
        public Order Order { get; set; }
        [DataMember]
        public Contact CreatedBy { get; set; }
        [DataMember]
        public List<Contact> AssignedTo { get; set; }
    }
}