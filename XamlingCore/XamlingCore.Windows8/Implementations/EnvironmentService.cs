using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using XamlingCore.Portable.Contract.Services;

namespace XamlingCore.Windows8.Implementations
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
