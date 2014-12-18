using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.Portable.Workflow.Annotations;
using XamlingCore.Portable.Workflow.Stage;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XFlowState : INotifyPropertyChanged, IEntity
    {
        private Guid _itemId;
        private XFlowStates _state;
        private bool _complete;
        private bool _previousStageSuccess;
        private int _failureCount;
        private bool _dismissed;
        private bool _isSuccessful;
        private DateTime _timestamp;
        private string _text;
        private string _stageId;
        public Guid Id { get; set; }

        private XStageResult _previousStageResult;

        public event EventHandler FlowCompleted;

        public XFlow ParentFlow { get; set; }

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

                if (value && FlowCompleted!=null)
                {
                    FlowCompleted(this, EventArgs.Empty);
                }

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

        public XStageResult PreviousStageResult
        {
            get { return _previousStageResult; }
            set
            {
                _previousStageResult = value;
                OnPropertyChanged();
            }
        }

        public bool IsSuccessful
        {
            get { return _isSuccessful; }
            set
            {
                _isSuccessful = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}