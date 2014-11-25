using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Workflow.Contract
{
    public interface IXFlow<in TEntityType> where TEntityType : IEntity
    {
        Task Add(TEntityType entity);
    }
}
