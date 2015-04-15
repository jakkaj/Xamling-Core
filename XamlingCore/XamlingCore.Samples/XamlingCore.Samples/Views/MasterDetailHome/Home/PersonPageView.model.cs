using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class PersonPageViewModel : XViewModel
    {
        private string _timeText;
        private string _name;
        public string TimeText
        {
            get { return _timeText; }
            set
            {
                _timeText = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                OnPropertyChanged();
            }
        }


        public override void OnInitialise()
        {
            base.OnInitialise();
            _init();
        }

        async void _init()
        {
            while (true)
            {
                TimeText = DateTime.Now.ToString();
                await Task.Delay(1000);
            }
        }
    }
}
