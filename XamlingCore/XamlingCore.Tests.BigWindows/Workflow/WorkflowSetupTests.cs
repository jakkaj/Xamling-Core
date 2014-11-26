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
        public async Task TestSetupPass()
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

                   if ((await activeFlow.GetInProgressItems()).Count == 0)
                   {
                       break;
                   }
               }
               

               
           });

           Assert.IsTrue((await activeFlow.GetInProgressItems()).Count == 0);
           Assert.IsTrue((await activeFlow.GetFailedItems()).Count == 0);
           Assert.IsTrue((await activeFlow.GetAllItems()).Count == 1);
        }

        [TestMethod]
        public async Task TestSetupFail()
        {


            var hub = Resolve<XWorkflowHub>();

            var flow = hub.AddFlow("TestFlow", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _failResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false);



            var activeFlow = await hub.Start("TestFlow", Guid.NewGuid());



            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    var f = activeFlow;

                    if ((await activeFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                }



            });

            Assert.IsTrue((await activeFlow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await activeFlow.GetFailedItems()).Count == 1);
            Assert.IsTrue((await activeFlow.GetAllItems()).Count == 1);


        }

        [TestMethod]
        public async Task TestPersist()
        {


            var hub = Resolve<XWorkflowHub>();

            Guid flowTemp = Guid.NewGuid();

            var flow = await hub.AddFlow(flowTemp.ToString(), "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _failResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();



            var activeFlow = await hub.Start(flowTemp.ToString(), Guid.NewGuid());



            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    var f = activeFlow;

                    if ((await activeFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                }



            });

            Assert.IsTrue((await activeFlow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await activeFlow.GetFailedItems()).Count == 1);
            Assert.IsTrue((await activeFlow.GetAllItems()).Count == 1);

            var flow2 = await hub.AddFlow(flowTemp.ToString(), "Nice name test flow")
               .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
               .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _failResult, false)
               .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
               .Complete();

            Assert.IsTrue((await flow2.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await flow2.GetFailedItems()).Count == 1);
            Assert.IsTrue((await flow2.GetAllItems()).Count == 1);
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
