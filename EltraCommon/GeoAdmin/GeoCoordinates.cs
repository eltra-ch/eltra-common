﻿using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.GeoAdmin
{
    [DataContract]
    public class GeoCoordinates
    {
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [IgnoreDataMember]
        public bool IsValid { get => Latitude > 0 && Longitude > 0; }
    }
}
