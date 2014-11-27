using System;

namespace XamlingCore.Portable.Workflow.Stage
{
    public class XStageResult
    {
        private readonly bool _isSuccess;
        private readonly Guid _id;
        private readonly string _extraText;
        private readonly bool _completeNow;
        private readonly string _exception;

        public XStageResult(bool isSuccess, Guid id, string extraText = null, bool completeNow = false, string exception = null)
        {
            _isSuccess = isSuccess;
            _id = id;
            _extraText = extraText;
            _completeNow = completeNow;
            _exception = exception;
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

        public string Exception
        {
            get { return _exception; }
        }
    }
}
