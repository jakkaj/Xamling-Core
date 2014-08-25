using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Services
{
    public interface IGoogleAnalyticsService
    {
        double DispatchInterval { get; set; }
        bool TrackUncaughtExceptions { get; set; }
        bool OptOut { get; set; }
        bool DryRun { get; set; }
        void SetView(string screenName);
        void SendEvent(string category, string actionText, string label);
        void SendTiming(string category, int interval, string name, string label);
        void SendException(string description, bool fatal);
    }
}
