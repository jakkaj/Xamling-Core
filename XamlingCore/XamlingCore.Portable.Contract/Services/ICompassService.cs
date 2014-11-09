using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Location;

namespace XamlingCore.Portable.Contract.Services
{
    public interface ICompassService
    {
        void Start();
        void Stop();
        XCompass CurrentCompassData { get; set; }
        event EventHandler CompassUpdated;
    }
}
