using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.Workflow.Flow;
using XamlingCore.Samples.Windows8.Workflow;

namespace XamlingCore.Samples.Windows8.View
{
    /// <summary>
    /// This class is not really intended to demonstrate the WF capabilities. 
    /// It's for development testing around resuming busted flows that were in progress when the app quits
    /// </summary>
    public class WorkflowViewModel : XViewModel
    {
        private readonly XWorkflowHub _hub;
        private readonly WorkflowExamples _wfExamples;

        public ICommand RunFlowsStagaA { get; set; }
        public ICommand RunFlowsStagaB { get; set; }

        private bool _alreayRunning;

        public WorkflowViewModel(XWorkflowHub hub, WorkflowExamples wfExamples)
        {
            _hub = hub;
            _wfExamples = wfExamples;

            RunFlowsStagaA = new Command(_runA);
            RunFlowsStagaB = new Command(_runB);
        }

        async void _runA()
        {
            await _wfExamples.SetupFlowsA();

            var activeFlow = await _hub.Start("TestFlowPass", Guid.NewGuid());

            Debug.WriteLine("Started flow A");
        }

        async void _runB()
        {
            var activeFlow = await _wfExamples.SetupFlowsB();

            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(5000);
                    var f = activeFlow;


                    //this should never end!
                    if ((await activeFlow.GetInProgressItems()).Count == 0)
                    {
                        break;
                    }
                }



            });

           Debug.WriteLine("*****Completed all the flows");
        }
    }
}
