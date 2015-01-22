using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Data.Glue;

namespace XamlingCore.XamarinThings.Content.Forms
{
    public class XLabel : Label
    {
        private ILocalisationService _localisationService;
        public XLabel()
        {
            _localisationService = ContainerHost.Container.Resolve<ILocalisationService>();
        }
        void _onSetText(string value)
        {
            Text = _localisationService.Get(value);

            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = "* No loc resource for " + value;
            }
        }
        public string XText
        {
            set
            {
                _onSetText(value);
            }
        }
    }
}
