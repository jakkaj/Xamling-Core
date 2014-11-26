using System;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XFlowState : IEntity
    {
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }
        public string StageId { get; set; }
        public XFlowStates State { get; set; }

        public bool Complete { get; set; }

        public bool PreviousStageSuccess { get; set; }

        public int FailureCount { get; set; }

        public bool Dismissed { get; set; }
        public DateTime Timestamp { get; set; }

        public string Text { get; set; }
    }
}