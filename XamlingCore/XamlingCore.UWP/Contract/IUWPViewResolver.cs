using System;
using XamlingCore.UWP.View;

namespace XamlingCore.UWP.Contract
{
    public interface IUWPViewResolver
    {
        XPage Resolve(object content);
        Type ResolvePageType(object content);
    }
}
