using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;

namespace XamlingCore.Samples.Views.Tile.MenuOption
{
    public class MenuOptionViewModel : ItemViewModel<XViewModel>
    {
        public MenuOptionViewModel()
        {
            Title = "MenuOpt";
        }
    }
}
