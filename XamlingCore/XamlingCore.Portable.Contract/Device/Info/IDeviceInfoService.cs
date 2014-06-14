using System;

namespace XamlingCore.Portable.Contract.Device.Info
{
    public interface IDeviceInfoService
    {
        bool IsLowMemory { get; }
        bool IsEmulator { get; }
        string Output { get; set; }
        int Percent { get; set; }
        void StartMemoryDebug(TimeSpan updateDelay);
    }
}