using System;
using XamlingCore.Portable.Model.Network;

namespace XamlingCore.Portable.Contract.Network
{
    public interface IDeviceNetworkStatus
    {
        event EventHandler NetworkChanged;
        bool QuickNetworkCheck();
        XNetworkType NetworkCheck();
    }   
}