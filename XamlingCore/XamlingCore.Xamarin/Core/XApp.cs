using Xamarin.Forms;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.XamarinThings.Core
{
    public abstract class XApplication : Xamarin.Forms.Application
    {
        public virtual void Init<TRootViewModel, TGlue>()
            where TRootViewModel : XViewModel
            where TGlue : class, IGlue, new()
        {
            var c = new XCore<TRootViewModel, TGlue>();

            var page = c.InitRoot();

            SetMainPage(page);
        }

        public void SetMainPage(Page page)
        {
            MainPage = page;
        }
    }
}