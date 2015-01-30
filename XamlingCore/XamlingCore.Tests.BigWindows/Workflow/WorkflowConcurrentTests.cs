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
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();


            var t = Task.Run(async () =>
            {
                var activeFlow = await hub.Start("TestFlowPass", Guid.NewGuid());
                while (true)
                {
                    var time = DateTime.Now;
                    await Task.Delay(1000);
                    

                    if ((await activeFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                    Assert.IsTrue(DateTime.Now.Subtract(time) < TimeSpan.FromSeconds(20));
                }



            });

            await Task.Delay(4000);

            var t2 = Task.Run(async () =>
            {
                var activeFlow = await hub.Start("TestFlowPass", Guid.NewGuid());
                while (true)
                {
                    
                    await Task.Delay(1000);
                    var time = DateTime.Now;
                   

                    if ((await activeFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                    Assert.IsTrue(DateTime.Now.Subtract(time) < TimeSpan.FromSeconds(20));
                }



            });

            await Task.Delay(1000);

            var t3 = Task.Run(async () =>
            {
                var activeFlow = await hub.Start("TestFlowPass", Guid.NewGuid());
                var time = DateTime.Now;

                while (true)
                {
                    await Task.Delay(1000);
                    

                    if ((await activeFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }

                    Assert.IsTrue(DateTime.Now.Subtract(time) < TimeSpan.FromSeconds(20));
                }



            });

            await Task.WhenAll(t, t2, t3);

        }
        async Task<XStageResult> _passResult(Guid itemId)
        {
            await Task.Delay(3000);
            Debug.WriteLine("Run Success Result: " + itemId);
            return new XStageResult(true, itemId);
        }
    }
}
