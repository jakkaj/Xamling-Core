using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Model.Network;

namespace XamlingCore.Tests.BigWindows.Impl
{
    public class WinMockDeviceNetworkStatus : IDeviceNetworkStatus
    {
        public bool HardcodeNetworkStatus { get; set; }

        public event EventHandler NetworkChanged;

        public WinMockDeviceNetworkStatus()
        {
            HardcodeNetworkStatus = true;
        }
        public bool QuickNetworkCheck()
        {
            return HardcodeNetworkStatus;
        }

        public XNetworkType NetworkCheck()
        {
            throw new NotImplementedException();
        }
    }
}
