using System;
using System.IO;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCoreServer.Steps;

namespace WorkflowCoreServer
{
    public class EventSampleWorkflow : IWorkflow<DataRelay>
    {
        public string Id => "EventSampleWorkflow";
            
        public int Version => 1;
        public WorkflowStatus Status => WorkflowStatus.Runnable;
            
        public void Build(IWorkflowBuilder<DataRelay> builder)
        {
            var branch1 = builder.CreateBranch()
                //.StartWith(context => ExecutionResult.Next())
                .StartWith<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: So sad you are in a bad mood today, Manuel.")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: Let me distract you with a riddle...")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => Letmegiveyouariddle());
            
            var branch2 = builder.CreateBranch()
                //.StartWith(context => ExecutionResult.Next())
                .StartWith<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: Glad you are in a good mood today, Manuel.")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: Let's toast to the good times...")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => Letmecheeryouup());
            
            builder
                .StartWith<SystemMessage>()
                    .Input(step => step.Message, data => "Workflow started...")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: Hello Manuel!")
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value1, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "Manuel replies: " + data.Value1)
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: How are you doing today?")
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value1, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "Manuel replies: " + data.Value1)
                    .Decide(data => data.Value1)
                        .Branch((data, outcome) => ReadMood(data.Value1) == "negative", branch1)
                        .Branch((data, outcome) => ReadMood(data.Value1) == "positive", branch2)
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value1, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "Manuel replies: " + data.Value1)
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: See how smart I am...Give me a city name of your choice!")
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value1, step => step.EventData)
                .Then<GetLocationInfo>()
                    .Input(step => step.Location, data => "Manuel replies: " + data.Value1)
                    .Output(data => data.Value1, step => step.Location)
                .Then<GetWeatherData>()
                    .Input(step => step.CityCode, data => data.Value1)
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: I have to go now.Bye!")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Workflow completed.");
                //.EndWorkflow();
        }

        private string Letmegiveyouariddle()
        {
            // read one entry randomly from IrishToasts.txt file
            var content = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Data\\Riddles.txt");
            string[] lines = content.Split('*');
            var rnd = new Random();
            var selInx = rnd.Next(0, lines.Count() - 1);
            return lines[selInx];
        }

        private string Letmecheeryouup()
        {
            // read one entry randomly from Riddles.txt file
            var content = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Data\\IrishToasts.txt");
            string[] lines = content.Split('*');
            var rnd = new Random();
            var selInx = rnd.Next(0, lines.Count() - 1);
            return lines[selInx];
        }

        private string ReadMood(string content)
        {
            //if (content.Contains("happy"))
            //    return "good";
            //if (content.Contains("crappy"))
            //    return "bad";
            //return "bad";
            var result = (new RunSentimentAnalysis()).SentimentInfo(content);
            return result.Result.ToLower();
            
        }
    }
}
