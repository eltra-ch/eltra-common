﻿using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.Enka.Orders
{
    [DataContract]
    public class JsonProtocolV1
    {
        [DataMember]
        public bool Car { get; set; }
        [DataMember]
        public bool DrugStore { get; set; }
        [DataMember]
        public string Notice { get; set; }
        [DataMember]
        public bool Other { get; set; }
        [DataMember]
        public bool Shop { get; set; }

    }
}
