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
    public class WorkflowConcurrentTests : TestBase
    {
        [TestMethod]
        public async Task TestConcurrencies()
        {
            var hub = Resolve<XWorkflowHub>();

            var flow = await hub.AddFlow("TestFlowPass", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Disconnected", "Stage 2...", "Stage 2 failed", null, true)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();


            var t = Task.Run(async () =>
            {
                var guid = Guid.NewGuid();
                var stage  = await hub.Start("TestFlowPass", guid);
                
                while (true)
                {
                    var time = DateTime.Now;
                    await Task.Delay(1000);
                    

                    if (stage.IsComplete)
                    {
                        break;
                    }

                    if (stage.State == XFlowStates.DisconnectedProcessing)
                    {
                        await Task.Delay(4000);
                        await stage.ParentFlow.ResumeDisconnected(stage.ItemId, true);
                    }

                    Assert.IsTrue(DateTime.Now.Subtract(time) < TimeSpan.FromSeconds(20));
                }
            });

            await Task.Delay(4000);

            var t2 = Task.Run(async () =>
            {
                var guid = Guid.NewGuid();
                var stage = await hub.Start("TestFlowPass", guid);

                while (true)
                {
                    var time = DateTime.Now;
                    await Task.Delay(4000);


                    if (stage.IsComplete)
                    {
                        break;
                    }

                    if (stage.State == XFlowStates.DisconnectedProcessing)
                    {
                        await Task.Delay(4000);
                        await stage.ParentFlow.ResumeDisconnected(stage.ItemId, true);
                    }

                    Assert.IsTrue(DateTime.Now.Subtract(time) < TimeSpan.FromSeconds(20));
                }


            });

            await Task.Delay(4000);

            var t3 = Task.Run(async () =>
            {
                var guid = Guid.NewGuid();
                var stage = await hub.Start("TestFlowPass", guid);

                while (true)
                {
                    var time = DateTime.Now;
                    await Task.Delay(1000);


                    if (stage.IsComplete)
                    {
                        break;
                    }

                    if (stage.State == XFlowStates.DisconnectedProcessing)
                    {
                        await Task.Delay(4000);
                        await stage.ParentFlow.ResumeDisconnected(stage.ItemId, true);
                    }

                    Assert.IsTrue(DateTime.Now.Subtract(time) < TimeSpan.FromSeconds(20));
                }



            });

            await Task.WhenAll(t, t2, t3);

        }
        async Task<XStageResult> _passResult(Guid itemId)
        {
            await Task.Delay(4000);
            Debug.WriteLine("Run Success Result: " + itemId);
            return new XStageResult(true, itemId);
        }
    }
}
