using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Data.Glue;

namespace XamlingCore.XamarinThings.Controls
{
    public class XEntry : Entry
    {
        private ILocalisationService _localisationService;
        public XEntry()
        {
            _localisationService = ContainerHost.Container.Resolve<ILocalisationService>();
        }

        void _onSetText(string value)
        {
            Placeholder = _localisationService.Get(value);

            if (string.IsNullOrWhiteSpace(Placeholder))
            {
                Placeholder = "* No loc resource for " + value;
            }
        }

        public string XPlaceholder
        {
            set
            {
                _onSetText(value);
            }
        }

        public string TextBind
        {
            set
            {
                SetBinding(TextProperty, new Binding(value, BindingMode.TwoWay));
            }
        }
    }
}
