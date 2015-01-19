using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Workflow.Flow;
using XamlingCore.Portable.Workflow.Stage;

namespace XamlingCore.Samples.Windows8.Workflow
{
    public class WorkflowExamples
    {
        private readonly XWorkflowHub _hub;

        public WorkflowExamples(XWorkflowHub hub)
        {
            _hub = hub;
        }

        public async Task SetupFlowsA()
        {
            var flow = await _hub.AddFlow("TestFlowPass", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Prepare", "Preparing2222...", "Preparation Failed", _passResult2, false)
                .AddStage("TestFlow.Prepare", "Stage 2...", "Stage 2 failed", _longResult, false, false, 2)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();
        }

        public async Task<XFlow> SetupFlowsB()
        {
            var flow = await _hub.AddFlow("TestFlowPass", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Prepare", "Preparing2222...", "Preparation Failed", _passResult2, false)
                .AddStage("TestFlow.Prepare", "Stage 2...", "Stage 2 failed", _secondTryResult, false, false, 2)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _finalPass, false)
                .Complete();

            return flow;
        }

        async Task<XStageResult> _longResult(Guid itemId)
        {
            Debug.WriteLine("Running long run flow task. You may exit");
            await Task.Delay(100000);
            Debug.WriteLine("Run Success Result: " + itemId);
            return new XStageResult(true, itemId);
        }
        async Task<XStageResult> _passResult(Guid itemId)
        {
            await Task.Delay(100);
            Debug.WriteLine("Run Success Result: " + itemId);
            return new XStageResult(true, itemId);
        }

        async Task<XStageResult> _passResult2(Guid itemId)
        {
            await Task.Delay(100);
            Debug.WriteLine("Run Success Result for Second a result: " + itemId);
            return new XStageResult(true, itemId);
        }

        async Task<XStageResult> _secondTryResult(Guid itemId)
        {
            await Task.Delay(100);
            Debug.WriteLine("Run second try Result: " + itemId);
            return new XStageResult(true, itemId);
        }

        async Task<XStageResult> _finalPass(Guid itemId)
        {
            await Task.Delay(100);
            Debug.WriteLine("*************************** Run _finalPass Result: " + itemId);
            return new XStageResult(true, itemId);
        }

    }
}
