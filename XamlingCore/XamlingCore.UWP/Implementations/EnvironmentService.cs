using System;
using Windows.UI.Xaml;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.UWP.Implementations
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetOSVersion()
        {
            throw new NotImplementedException();
        }

        public double GetScreenWidth()
        {
            return Window.Current.Bounds.Width;
        }

        public double GetScreenHeight()
        {
            return Window.Current.Bounds.Height;
        }

        public float GetScreenScale()
        {
            throw new NotImplementedException();
        }

        public string GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}
