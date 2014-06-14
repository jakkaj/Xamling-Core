using System;

namespace XamlingCore.Portable.Contract.Network
{
    public interface IDeviceNetworkStatus
    {
        event EventHandler NetworkChanged;
        bool QuickNetworkCheck();
    }   
}