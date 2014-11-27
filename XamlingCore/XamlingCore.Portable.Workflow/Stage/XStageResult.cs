using System;

namespace XamlingCore.Portable.Workflow.Stage
{
    public class XStageResult
    {
        private readonly bool _isSuccess;
        private readonly Guid _id;
        private readonly string _extraText;
        private readonly bool _completeNow;

        public XStageResult(bool isSuccess, Guid id, string extraText = null, bool completeNow = false)
        {
            _isSuccess = isSuccess;
            _id = id;
            _extraText = extraText;
            _completeNow = completeNow;
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public string ExtraText
        {
            get { return _extraText; }
        }

        public bool CompleteNow
        {
            get { return _completeNow; }
        }
    }
}
