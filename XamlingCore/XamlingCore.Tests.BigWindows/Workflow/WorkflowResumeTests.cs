using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using XamlingCore.Portable.Workflow.Flow;
using XamlingCore.Portable.Workflow.Stage;
using XamlingCore.Tests.BigWindows.Base;

namespace XamlingCore.Tests.BigWindows.Workflow
{
    [TestClass]
    public class WorkflowResumeTests : TestBase
    {
        public async Task SetupFlow()
        {
            var hub = Resolve<XWorkflowHub>();

            var flow = await hub.AddFlow("TestFlowPass", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _longResult, false, false, 2)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();
            


            var activeFlow = await hub.Start("TestFlowPass", Guid.NewGuid());
        }

        public async Task ResumeFlow()
        {
            var hub = Resolve<XWorkflowHub>();

            var flow = await hub.AddFlow("TestFlowPass", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, false, false, 2)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();



            var activeFlow = await hub.Start("TestFlowPass", Guid.NewGuid());
        }


        async Task<XStageResult> _longResult(Guid itemId)
        {
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

        public async Task ContinueFlow()
        {
            
        }

        async void _configureHub()
        {
            
        }
    }
}
