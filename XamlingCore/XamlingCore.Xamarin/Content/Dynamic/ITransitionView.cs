using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.XamarinThings.Content.Dynamic
{
    public interface ITransitionView
    {
        Task TransitionOut();
        void TransitionIn();
    }
}
