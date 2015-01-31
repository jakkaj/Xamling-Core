using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Workflow.Flow;
using XamlingCore.Portable.Workflow.Stage;
using XamlingCore.Tests.BigWindows.Base;
using XamlingCore.Tests.BigWindows.Impl;

namespace XamlingCore.Tests.BigWindows.Workflow
{
    [TestClass]
    public class WorkflowConnectivityTests : TestBase
    {
        [TestMethod]
        public async Task TestOfflineOnlineTransition()
        {


            var hub = Resolve<XWorkflowHub>();

            var network = Resolve<IDeviceNetworkStatus>() as WinMockDeviceNetworkStatus;

            network.HardcodeNetworkStatus = false;

            

            var flow = await hub.AddFlow("TestFlowPass", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult,false, true, 2)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();



           
            var activeFlowState = await hub.Start("TestFlowPass", Guid.NewGuid());
            var activeFlow = activeFlowState.ParentFlow;


            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(5000);
                    var f = activeFlow;

                    network.HardcodeNetworkStatus = true;

                    if ((await activeFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                }



            });

            Assert.IsTrue((await activeFlow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await activeFlow.GetFailedItems()).Count == 0);
            Assert.IsTrue((await activeFlow.GetAllItems()).Count >= 1);
        }

        async Task<XStageResult> _failResult(Guid itemId)
        {
            await Task.Delay(500);
            Debug.WriteLine("Run Fail Result");
            return new XStageResult(false, itemId);
        }

        async Task<XStageResult> _passResult(Guid itemId)
        {
            await Task.Delay(100);
            Debug.WriteLine("Run Success Result: " + itemId);
            return new XStageResult(true, itemId);
        }
    }
}
