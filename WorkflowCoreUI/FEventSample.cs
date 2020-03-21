using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO.Pipes;
using System.Security.Principal;
using System.Windows.Forms;
using WorkflowCore.Interface;

namespace WorkflowCoreUI
{
    public partial class FEventSample : Form
    {
        private IWorkflowHost host;

        public FEventSample()
        {
            InitializeComponent();
        }

        private void FEventSample_Load(object sender, EventArgs e)
        {
            IServiceProvider serviceProvider = ConfigureServices();

            //start the workflow host
            host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<EventSampleWorkflow, MyDataClass>();
            host.Start();

            var initialData = new MyDataClass();

            //var workflowId = host.StartWorkflow("EventSampleWorkflow", 1, initialData).Result;
            tbMyText.Enabled = true;
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            var coll = services.AddWorkflow(x => x.UseSqlServer(@"Server=.\MSSQLSERVER2;Database=WorkflowCore;Trusted_Connection=True;", true, true));
            
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        
        private void bnSubmit_Click(object sender, EventArgs e)
        {
            host.PublishEvent("MyEvent", tbWorkflowId.Text, tbMyText.Text);
        }

        private void bnGetWorkflowId_Click(object sender, EventArgs e)
        {
            var pipeClient =
                        new NamedPipeClientStream(".", "testpipe",
                            PipeDirection.InOut, PipeOptions.None,
                            TokenImpersonationLevel.Impersonation);

            Console.WriteLine("Connecting to server...\n");
            pipeClient.Connect();

            var ss = new StreamString(pipeClient);
            // Validate the server's signature string.
            var wkflMsg = ss.ReadString();
            tbWorkflowId.Text = wkflMsg.Split('=')[1];
        }
    }
}
