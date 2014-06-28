using System.Reflection;
using XamlingCore.Portable.Glue.Glue;


namespace XamlingCore.Samples.iOS.Glue.Modules
{
    public class ViewsModule : XAutofacModule
    {
        public ViewsModule()
            : base(Assembly.Load("XamlingCore.Samples"), "View", true, false)
        {
            
        }
    }
}
