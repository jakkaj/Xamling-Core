using System.Collections.ObjectModel;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.Samples.View.Tile.MenuOption;

namespace XamlingCore.Samples.View.MasterDetailHome.Menu
{
    public class MasterDetailMenuPageViewModel : DisplayListViewModel<MenuOptionViewModel, XViewModel>
    {
        public override void OnActivated()
        {
            _refresh();
            base.OnActivated();
        }

        void _refresh()
        {
            if (DataList == null)
            {
                return;
            }

            Items = new ObservableCollection<MenuOptionViewModel>();

            foreach (var item in DataList)
            {
                var i = CreateContentModel<MenuOptionViewModel>(_ => _.Item = item);

                Items.Add(i);
            }

            UpdateItemCount();
        }
    }
}
