using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Windows8.View
{
    public class HomeViewModel : XViewModel
    {
        public ICommand NextPageCommand { get; set; }

        public ICommand WorkflowsCommand { get; set; }

        private string _text;

        public HomeViewModel()
        {
            NextPageCommand = new Command(_onNextPage);
            WorkflowsCommand = new Command(_onWorkflows);
            Text = "This is a test";
        }

        void _onWorkflows()
        {
            NavigateTo<WorkflowViewModel>();
        }

        void _onNextPage()
        {
            NavigateTo<AnotherViewModel>();
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}
