using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.DTO.Localisation;

namespace XamlingCore.Portable.Contract.Localisation
{
    public interface IXLocalisable
    {
        string XResourceId { get; set; }
        XLocalisedResources XLocalisedResources { get; set; }
    }
}
