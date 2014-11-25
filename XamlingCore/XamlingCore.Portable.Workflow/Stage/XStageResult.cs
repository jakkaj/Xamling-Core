using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Workflow.Stage
{
    public class XStageResult<TEntityType> where TEntityType : IEntity
    {
        private readonly bool _isSuccess;
        private readonly TEntityType _entity;

        public XStageResult(bool isSuccess, TEntityType entity)
        {
            _isSuccess = isSuccess;
            _entity = entity;
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
        }

        public TEntityType Entity
        {
            get { return _entity; }
        }
    }
}
