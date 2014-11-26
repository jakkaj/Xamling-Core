using System;

namespace XamlingCore.Portable.Workflow.Stage
{
    public class XStageResult
    {
        private readonly bool _isSuccess;
        private readonly Guid _id;

        public XStageResult(bool isSuccess, Guid id)
        {
            _isSuccess = isSuccess;
            _id = id;
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}
