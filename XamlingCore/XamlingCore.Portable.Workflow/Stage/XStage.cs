using System;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Workflow.Stage
{
    public class XStage
    {
        public Func<Guid, Task<XStageResult>> Function { get; set; }
        
        public bool RequiresNetwork { get; set; }

        public int Retries { get; set; }

        public string StageId { get; set; }

        public string ProcessingText { get; set; }

        public string FailText { get; set; }
       
        public bool IsDisconnectedProcess { get; set; }

        public XStage(string stageId)
        {
            StageId = stageId;
        }

        public XStage(string stageId, string processingText, string failText, Func<Guid, Task<XStageResult>> function,
            bool isDisconnectedProcess = false, bool requiresNetwork = false, int retries = 0)
        {
            IsDisconnectedProcess = isDisconnectedProcess;
            StageId = stageId;
            ProcessingText = processingText;
            FailText = failText;
            Function = function;
            RequiresNetwork = requiresNetwork;
            Retries = retries;
        }

        public async Task<XStageResult> Process(Guid id)
        {
            if (Function == null) throw new NullReferenceException(_getErrorString("Processor function callback not set"));

            var result = await Function(id);

            return result;
        }
      
        string _getErrorString(string text)
        {
            return string.Format("[{0}] - Processor - {1}", StageId ?? "NoNameStage", text);
        }
    }
}
