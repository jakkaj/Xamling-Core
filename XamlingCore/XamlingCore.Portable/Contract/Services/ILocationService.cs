using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Portable.Contract.Services
{
    public interface ILocationService
    {
        event EventHandler LocationUpdated;

        void Start();
        void Stop();

        XLocation GetCurrentLocation();
        Task<double?> Distance(double lat, double lng);

        XLocation CurrentLocation { get; }
        Task<bool> IsLocationEnabled();
        Task<XLocation> GetQuickLocation();
        bool IsLocationResolved();
        bool IsLocationEnabledInDeviceSettings();
    }
}
