using System;
using System.IO;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCoreServer.Steps;

namespace WorkflowCoreServer
{
    public class EventSampleWorkflow : IWorkflow<MyDataClass>
    {
        public string Id => "EventSampleWorkflow";
            
        public int Version => 1;
            
        public void Build(IWorkflowBuilder<MyDataClass> builder)
        {
            var branch1 = builder.CreateBranch()
                .StartWith(context => ExecutionResult.Next())
                .Then(context => Console.WriteLine("Otto says: So sad you feel crappy today, Manuel."))
                .Then(context => Console.WriteLine("Otto says: Let me distract you with a riddle..."))
                .Then(context => Letmegiveyouariddle());
            
            var branch2 = builder.CreateBranch()
                .StartWith(context => ExecutionResult.Next())
                .Then(context => Console.WriteLine("Otto says: Glad you feel happy today, Manuel."))
                .Then(context => Console.WriteLine("Otto says: Let's toast to the good times..."))
                .Then(context => Letmecheeryouup()); 

            builder
                .StartWith(context => ExecutionResult.Next())
                .Then(context => Console.WriteLine("Otto says: Hello Manuel!"))
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value1, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "Manuel replies: " + data.Value1)
                .Then(context => Console.WriteLine("Otto says: How are you doing today?"))
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value1, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "Manuel replies: " + data.Value1)
                    .Decide(data => data.Value1)
                        .Branch((data, outcome) => data.Value1.Contains("crappy"), branch1)
                        .Branch((data, outcome) => data.Value1.Contains("happy"), branch2);
        }

        private void Letmegiveyouariddle()
        {
            // read one entry randomly from IrishToasts.txt file
            var content = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Data\\Riddles.txt");
            string[] lines = content.Split('*');
            var rnd = new Random();
            var selInx = rnd.Next(0, lines.Count() - 1);
            Console.WriteLine(lines[selInx]);

        }

        private void Letmecheeryouup()
        {
            // read one entry randomly from Riddles.txt file
            var content = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Data\\IrishToasts.txt");
            string[] lines = content.Split('*');
            var rnd = new Random();
            var selInx = rnd.Next(0, lines.Count() - 1);
            Console.WriteLine(lines[selInx]);
        }
    }
}
