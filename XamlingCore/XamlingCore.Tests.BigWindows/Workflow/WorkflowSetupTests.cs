using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task TestMergeFlows()
        {


            var hub = Resolve<XWorkflowHub>();

            var idFlow = Guid.NewGuid();
            var idFlow2 = Guid.NewGuid();
            var flow = await hub.AddFlow(idFlow.ToString(), "Nice name test flow")
                .AddStage("TestFlow.Preparea", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2a", "Stage 2...", "Stage 2 failed", _passResult, true)
                .AddStage("TestFlow.FinalStagea", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();

            var flow2 = await hub.AddFlow(idFlow2.ToString(), "Nice name test flow")
                .AddStage("TestFlow2.Prepare2", "Preparing...", "Preparation Failed", _passResult, false)
                .Merge(flow)
                .AddStage("TestFlow.FinalStage2", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();


            var id = Guid.NewGuid();
            
            var activeFlowState = await hub.Start(idFlow2.ToString(), id);
            var activeFlow = activeFlowState.ParentFlow;
            var state = activeFlow.GetState(id);

            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);

                    if (state.State == XFlowStates.DisconnectedProcessing)
                    {
                        await activeFlow.ResumeDisconnected(id, true);
                    }

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


        [TestMethod]
        public async Task TestSetupDisconnect()
        {


            var hub = Resolve<XWorkflowHub>();

            var idFlow = Guid.NewGuid();

            var flow = await hub.AddFlow(idFlow.ToString(), "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, true)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();


            var id = Guid.NewGuid();
            var activeFlow = await hub.Start(idFlow.ToString(), id);

            var state = activeFlow.ParentFlow.GetState(id);

            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);

                    if (state.State == XFlowStates.DisconnectedProcessing)
                    {
                        await activeFlow.ParentFlow.ResumeDisconnected(id, true);
                    }

                    if ((await activeFlow.ParentFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                }



            });

            Assert.IsTrue((await activeFlow.ParentFlow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await activeFlow.ParentFlow.GetFailedItems()).Count == 0);
            Assert.IsTrue((await activeFlow.ParentFlow.GetAllItems()).Count >= 1);
        }

        [TestMethod]
        public async Task TestSetupPass()
        {


            var hub = Resolve<XWorkflowHub>();

            var flow = await hub.AddFlow("TestFlowPass", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();



            var activeFlow = await hub.Start("TestFlowPass", Guid.NewGuid());



            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    var f = activeFlow;

                    if ((await activeFlow.ParentFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                }



            });

            Assert.IsTrue((await activeFlow.ParentFlow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await activeFlow.ParentFlow.GetFailedItems()).Count == 0);
            Assert.IsTrue((await activeFlow.ParentFlow.GetAllItems()).Count >= 1);
        }

        [TestMethod]
        public async Task TestSetupFail()
        {


            var hub = Resolve<XWorkflowHub>();

            var flow = await hub.AddFlow("TestFlowFail", "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _failResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();

            var activeFlow = await hub.Start("TestFlowFail", Guid.NewGuid());



            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    var f = activeFlow;

                    if ((await activeFlow.ParentFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                }



            });

            Assert.IsTrue((await activeFlow.ParentFlow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await activeFlow.ParentFlow.GetFailedItems()).Count >= 1);
            Assert.IsTrue((await activeFlow.ParentFlow.GetAllItems()).Count >= 1);


        }

        //[TestMethod]
        //public async Task TestPersist()
        //{


        //    var hub = Resolve<XWorkflowHub>();

        //    Guid flowTemp = Guid.NewGuid();

        //    var flow = await hub.AddFlow(flowTemp.ToString(), "Nice name test flow")
        //        .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
        //        .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, false)
        //        .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
        //        .Complete();

        //    var itemGuid = Guid.NewGuid();


        //    var activeFlow = await hub.Start(flowTemp.ToString(), itemGuid);

        //    var state = activeFlow.GetState(itemGuid);
        //    state.PropertyChanged += state_PropertyChanged;
        //    await Task.Run(async () =>
        //    {
        //        while (true)
        //        {


        //            if (state.State == XFlowStates.Success)
        //            {
        //                Assert.AreEqual(state.StageId, "TestFlow.FinalStage");
        //                Assert.AreEqual(state.Text, "");
        //                Assert.AreEqual((await activeFlow.GetInProgressItems()).Count, 0);
        //                break;
        //            }


        //            //Debug.WriteLine("StageId: {0}, Text: {1}", state.StageId, state.Text);


        //            await Task.Yield();
        //        }



        //    });

        //    Assert.IsTrue((await activeFlow.GetInProgressItems()).Count == 0);
        //    Assert.IsTrue((await activeFlow.GetFailedItems()).Count == 0);
        //    Assert.IsTrue((await activeFlow.GetAllItems()).Count == 1);
        //    var hub2 = Resolve<XWorkflowHub>();
        //    var flow2 = await hub2.AddFlow(flowTemp.ToString(), "Nice name test flow")
        //       .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
        //       .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _failResult, false)
        //       .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
        //       .Complete();

        //    Assert.IsTrue((await flow2.GetInProgressItems()).Count == 0);
        //    Assert.IsTrue((await flow2.GetFailedItems()).Count == 0);
        //    Assert.IsTrue((await flow2.GetAllItems()).Count == 1);
        //}


        [TestMethod]
        public async Task TestPersistMultiples()
        {


            var hub = Resolve<XWorkflowHub>();

            Guid flowTemp = Guid.NewGuid();

            var flow = await hub.AddFlow(flowTemp.ToString(), "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();
            var ids = new List<Guid>();
            
            for (var i = 0; i < 20; i++)
            {
                Debug.WriteLine("Testing: {0}", i);
                var g = Guid.NewGuid();
                ids.Add(g);
                var activeFlow = await hub.Start(flowTemp.ToString(), g);
                var state = activeFlow.ParentFlow.GetState(g);
                await Task.Run(async () =>
                {
                    while (true)
                    {


                        if (state.State == XFlowStates.Success)
                        {
                           
                            break;
                        }


                        //Debug.WriteLine("StageId: {0}, Text: {1}", state.StageId, state.Text);


                        await Task.Yield();
                    }



                });
            }

            Assert.IsTrue((await flow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await flow.GetFailedItems()).Count == 0);
            Assert.IsTrue((await flow.GetAllItems()).Count == 20);

            foreach (var item in ids)
            {
                var thisState = flow.GetState(item);
                Assert.AreEqual(thisState.State, XFlowStates.Success);

            }


           
        }

        [TestMethod]
        public async Task TestMultiplesParallel()
        {


            var hub = Resolve<XWorkflowHub>();

            Guid flowTemp = Guid.NewGuid();

            var flow = await hub.AddFlow(flowTemp.ToString(), "Nice name test flow")
                .AddStage("TestFlow.Prepare", "Preparing...", "Preparation Failed", _passResult, false)
                .AddStage("TestFlow.Stage2", "Stage 2...", "Stage 2 failed", _passResult, false)
                .AddStage("TestFlow.FinalStage", "Stage final...", "Stage final failed", _passResult, false)
                .Complete();
            var ids = new List<Guid>();

            for (var i = 0; i < 20; i++)
            {
                Debug.WriteLine("Testing: {0}", i);
                var g = Guid.NewGuid();
                ids.Add(g);
                var activeFlow = await hub.Start(flowTemp.ToString(), g);
                var state = activeFlow.ParentFlow.GetState(g);
                
            }

            await Task.Run(async () =>
            {
                while (true)
                {


                    if ((await flow.GetInProgressItems()).Count == 0)
                    {

                        break;
                    }


                    //Debug.WriteLine("StageId: {0}, Text: {1}", state.StageId, state.Text);


                    await Task.Delay(500);
                }



            });

            Assert.IsTrue((await flow.GetInProgressItems()).Count == 0);
            Assert.IsTrue((await flow.GetFailedItems()).Count == 0);
            Assert.IsTrue((await flow.GetAllItems()).Count == 20);

            foreach (var item in ids)
            {
                var thisState = flow.GetState(item);
                Assert.AreEqual(thisState.State, XFlowStates.Success);

            }



        }

        void state_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                var ty = sender as XFlowState;
                Debug.WriteLine("New Text: {0}", ty.Text);
            }
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
