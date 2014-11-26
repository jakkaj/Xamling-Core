using System;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.View.ViewModel.Base;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XFlowState : NotifyBase, IEntity
    {
        private Guid _itemId;
        private XFlowStates _state;
        private bool _complete;
        private bool _previousStageSuccess;
        private int _failureCount;
        private bool _dismissed;
        private DateTime _timestamp;
        private string _text;
        private string _stageId;
        public Guid Id { get; set; }

        public Guid ItemId
        {
            get { return _itemId; }
            set
            {
                _itemId = value;
                OnPropertyChanged();
            }
        }

        public string StageId
        {
            get { return _stageId; }
            set
            {
                _stageId = value;
                OnPropertyChanged();
            }
        }

        public XFlowStates State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public bool Complete
        {
            get { return _complete; }
            set
            {
                _complete = value;
                OnPropertyChanged();
            }
        }

        public bool PreviousStageSuccess
        {
            get { return _previousStageSuccess; }
            set
            {
                _previousStageSuccess = value;
                OnPropertyChanged();
            }
        }

        public int FailureCount
        {
            get { return _failureCount; }
            set
            {
                _failureCount = value;
                OnPropertyChanged();
            }
        }

        public bool Dismissed
        {
            get { return _dismissed; }
            set
            {
                _dismissed = value;
                OnPropertyChanged();
            }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}