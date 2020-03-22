using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCoreUI;

namespace WorkflowCoreUI
{
    public class EventSampleWorkflow : IWorkflow<MyDataClass>
    {
        public string Id => "EventSampleWorkflow";

        public FEventSample FSubscriber { get; set; }

        public int Version => 1;
            
        public void Build(IWorkflowBuilder<MyDataClass> builder)
        {
            //builder
            //    .StartWith(context => ExecutionResult.Next())
            //    .Then(context => Console.WriteLine("workflow started"))
            //    .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
            //        .Output(data => data.Value1, step => step.EventData)
            //    .Then<CustomMessage>()
            //        .Input(step => step.Message, data => "The data from the event is " + data.Value1)
            //    .Then(context => Console.WriteLine("workflow complete"));
            var msg = "hey";
        }
    }
}
