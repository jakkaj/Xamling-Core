using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Workflow.Flow;
using XamlingCore.Portable.Workflow.Stage;
using XamlingCore.Tests.BigWindows.Base;

namespace XamlingCore.Tests.BigWindows.Workflow
{
    [TestClass]
    public class WorkflowSetupTests : TestBase
    {
       

        [TestMethod]
        public async Task TestSetup()
        {
           

            var hub = Resolve<XWorkflowHub>();

            var flow = hub.AddFlow("TestFlow", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false);



            var activeFlow = await hub.Start("TestFlow", Guid.NewGuid());

           

           await  Task.Run(async () =>
           {
               while (true)
               {
                   await Task.Delay(1000);
                   var f = activeFlow;

                   if (f.InProgressItems.Count == 0)
                   {
                       break;
                   }
               }
               

               
           });

           Assert.IsTrue(activeFlow.InProgressItems.Count == 0);

            
        }

        async Task<XStageResult> _failResult(Guid itemId)
        {
            await Task.Delay(500);

            return new XStageResult(false, itemId);
        }

        async Task<XStageResult> _passResult(Guid itemId)
        {
            await Task.Delay(500);

            return new XStageResult(true, itemId);
        }
    }
}
