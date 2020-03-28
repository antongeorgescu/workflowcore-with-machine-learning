using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
                    .Input(step => step.Message, data => "Otto says: Let's cheer for the good times...")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => Letmecheeryouup());

            var branch3 = builder.CreateBranch()
                .StartWith<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: Good hearing from you, Manuel.")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => "Otto says: Let's toast to better times...")
                .Then<SystemMessage>()
                    .Input(step => step.Message, data => Letmegetacoktailforya());

            builder
                .StartWith<SystemMessage>()
                    .Input(step => step.Message, data => "\tSystem says: Workflow started...")
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
                        .Branch((data, outcome) => ReadMood(data.Value1) == "undecided", branch3)
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
                    .Input(step => step.Message, data => $"\tSystem says: Workflow completed.\n\tSystem says: Hit any key to terminate the workflow...")
                .EndWorkflow();
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

        private string Letmegetacoktailforya()
        {
            string url = @"https://www.thecocktaildb.com/api/json/v1/1/search.php?f=a";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "GET"; // or "POST", "PUT", "PATCH", "DELETE", etc.

            string result = "";
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Do something with the response
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Get a reader capable of reading the response stream
                    using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        // Read stream content as string
                        string responseJSON = myStreamReader.ReadToEnd();

                        // Assuming the response is in JSON format, deserialize it
                        // creating an instance of TData type (generic type declared before).
                        var oresult = JsonConvert.DeserializeObject<object>(responseJSON);

                        var getrandom = new Random();
                        int inx = getrandom.Next(0, ((dynamic)JsonConvert.DeserializeObject(responseJSON)).drinks.Count - 1);

                        var drinkName = ((dynamic)JsonConvert.DeserializeObject(responseJSON)).drinks[inx].strDrink;
                        var drinkInstructions = ((dynamic)JsonConvert.DeserializeObject(responseJSON)).drinks[inx].strInstructions;
                        result = $"{drinkName} | {drinkInstructions}";
                    }
                }
            }
            return result;
        }

        private static async Task<string> Getacocktail()
        {
            var requestUri = @"https://www.thecocktaildb.com/api/json/v1/1/search.php?f=a";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        private string ReadMood(string content)
        {
            string result;
            try
            {
                //var asyncResult = (new RunSentimentAnalysis()).SentimentInfoAsync(content);
                //result = asyncResult.Result.ToLower();

                result = (new RunSentimentAnalysis()).SentimentInfo(content).ToLower();
            
            }
            catch(Exception ex)
            {
                result = "undecided";
            }
            return result; 
        }
    }
}
