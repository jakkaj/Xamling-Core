using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Transitions
{
    public class TransitionViewModel : XViewModel
    {
        private XViewModel _contentViewModel;

        private XViewModel _content1;
        private XViewModel _content2;

        public TransitionViewModel()
        {
            Title = "Transitions";
        }

        public override void OnInitialise()
        {
            base.OnInitialise();

            _content1 = CreateContentModel<Content1ViewModel>();
            _content2 = CreateContentModel<Content2ViewModel>();

            
        }

        public override void OnActivated()
        {
            base.OnActivated();

            _switch();
        }

        async void _switch()
        {
            while (true)
            {
                if (!(ContentViewModel is Content1ViewModel))
                {
                    ContentViewModel = _content1;
                }
                else
                {
                    ContentViewModel = _content2;
                }
                await Task.Delay(4000);
            }
            
        }

        public XViewModel ContentViewModel
        {
            get { return _contentViewModel; }
            set
            {
                _contentViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}
