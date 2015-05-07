using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.View.Special;
using XamlingCore.Windows8.Controls;

namespace XamlingCore.Windows8.Implementations
{
    public class LoadStatusService : LoadStatusServiceBase
    {
        private Canvas _rootCanvas;


        public LoadStatusService(IDispatcher dispatcher)
            : base(dispatcher)
        {
        }


        public override void ShowIndicator(string text)
        {
            var v = Window.Current.Content as UserControl;
            if (v == null)
            {
                return;
            }
            var newLoader = new LoaderOverlayView();


            var c = v.Content as Canvas;

            if (c != null)
            {
                _rootCanvas = c;
                c.Children.Add(newLoader);

                newLoader.SetValue(Canvas.LeftProperty, (c.ActualWidth - newLoader.Width) / 2);
                newLoader.SetValue(Canvas.TopProperty, (c.ActualHeight - newLoader.Height) / 2);
            }
            
        }

        public override void HideIndicator()
        {
            if (_rootCanvas != null)
            {
                var loader = _rootCanvas.Children.FirstOrDefault(_ => _ is LoaderOverlayView);
                if (loader != null)
                {
                    _rootCanvas.Children.Remove(loader);
                }
            }
        }

        void _showFullscreen()
        {

        }

        void _hideFullscreen()
        {

        }
    }
}
