using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCoreServer.NamedPipesHelpers;

namespace WorkflowCoreServer
{
    public class Program
    {
        private static int numThreads = 4;
        private static string workflowId;
        private static Thread workflow;

        public static void Main(string[] args)
        {
            workflow = new Thread(RunWorkflow);
            workflow.Start();

            // Start namedpipes server
            NamedPipeServer();
        }

        private static void RunWorkflow()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            //start the workflow host
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<EventSampleWorkflow, DataRelay>();
            host.Start();

            var initialData = new DataRelay();
            workflowId = host.StartWorkflow("EventSampleWorkflow", 1, initialData).Result;
            Console.WriteLine($"\tWorkflow ID: {workflowId}");
            
            Console.ReadLine();
            
            var result = host.TerminateWorkflow(workflowId).Result;
            if (result)
                Console.WriteLine($"\tSystem says: Workflow {workflowId} terminated with status SUCCESS");
            else
                Console.WriteLine($"\tSystem says: Workflow {workflowId} terminated with status FAILED");
            host.Stop();
        }

        public static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            //services.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            services.AddWorkflow(x => x.UseSqlServer(@"Server=.\MSSQLSERVER2;Database=WorkflowCore;Trusted_Connection=True;", true, true));
            //services.AddWorkflow(x => x.UsePostgreSQL(@"Server=127.0.0.1;Port=5432;Database=workflow;User Id=postgres;", true, true));
            //services.AddWorkflow(x => x.UseSqlite(@"Data Source=database.db;", true));            

            //services.AddWorkflow(x =>
            //{
            //    x.UseAzureSynchronization(@"UseDevelopmentStorage=true");
            //    x.UseMongoDB(@"mongodb://localhost:27017", "workflow9999");
            //});

            //services.AddWorkflow(x =>
            //{
            //    x.UseSqlServer(@"Server=.\SQLEXPRESS;Database=WorkflowCore;Trusted_Connection=True;", true, true);
            //    x.UseSqlServerLocking(@"Server=.\SQLEXPRESS;Database=WorkflowCore;Trusted_Connection=True;");
            //});

            //services.AddWorkflow(cfg =>
            //{
            //    var ddbConfig = new AmazonDynamoDBConfig() { RegionEndpoint = RegionEndpoint.USWest2 };

            //    cfg.UseAwsDynamoPersistence(new EnvironmentVariablesAWSCredentials(), ddbConfig, "sample4");
            //    cfg.UseAwsDynamoLocking(new EnvironmentVariablesAWSCredentials(), ddbConfig, "workflow-core-locks");
            //    cfg.UseAwsSimpleQueueService(new EnvironmentVariablesAWSCredentials(), new AmazonSQSConfig() { RegionEndpoint = RegionEndpoint.USWest2 });                
            //});

            //services.AddWorkflow(cfg =>
            //{
            //    cfg.UseRedisPersistence("localhost:6379", "sample4");
            //    cfg.UseRedisLocking("localhost:6379");
            //    cfg.UseRedisQueues("localhost:6379", "sample4");
            //    cfg.UseRedisEventHub("localhost:6379", "channel1");
            //});

            //services.AddWorkflow(x =>
            //{
            // x.UseMongoDB(@"mongodb://192.168.0.12:27017", "workflow");
            //x.UseRabbitMQ(new ConnectionFactory() { HostName = "localhost" });
            //x.UseRedlock(redis);
            //});


            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }


        private static void NamedPipeServer()
        {
            int i;
            Thread[] servers = new Thread[numThreads];

            Console.WriteLine("\n\t*** Named pipe server stream with impersonation example ***\n");
            Console.WriteLine("\tWaiting for client connect...\n");
            for (i = 0; i < numThreads; i++)
            {
                servers[i] = new Thread(ServerThread);
                servers[i].Start();
            }
            Thread.Sleep(250);
            while (i > 0)
            {
                for (int j = 0; j < numThreads; j++)
                {
                    if (servers[j] != null)
                    {
                        if (servers[j].Join(250))
                        {
                            Console.WriteLine("\tServer thread[{0}] finished.", servers[j].ManagedThreadId);
                            servers[j] = null;
                            i--;    // decrement the thread watch count
                        }
                    }
                }
            }
            Console.WriteLine("\n\tServer threads exhausted, exiting.");
        }

        private static void ServerThread(object data)
        {
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("testpipe", PipeDirection.InOut, numThreads);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            Console.WriteLine("\tClient connected on thread[{0}].", threadId);
            try
            {
                // Read the request from the client. Once the client has
                // written to the pipe its security token will be available.

                StreamString ss = new StreamString(pipeServer);

                // Verify our identity to the connected client using a
                // string that the client anticipates.

                //ss.WriteString("I am the one true server!");
                ss.WriteString($"WorkflowId={workflowId}");
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
            pipeServer.Close();
        }

    }
}
