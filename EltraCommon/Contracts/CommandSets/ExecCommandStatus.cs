﻿namespace EltraCommon.Contracts.CommandSets
{
    public enum ExecCommandStatus
    {
        Registered,
        Executing,
        Failed,
        Waiting,
        Executed,
        Refused,
        Complete,
        Announced
    }
}