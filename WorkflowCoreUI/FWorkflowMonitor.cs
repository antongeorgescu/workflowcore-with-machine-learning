using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorkflowCoreUI
{
    public partial class FWorkflowMonitor : Form
    {
        public FWorkflowMonitor()
        {
            InitializeComponent();

            //dgvwWorkflows.DataSource = new List<object>
            //{
            //    new { Id = 1, Name = "The Very Big Corporation of America" },
            //    new { Id = 2, Name = "Old Accountants Ltd" }
            //};

            textBox1.AppendText("Copy the following 2 lines of Sql script and run them where WorkflowCore persistance database is deployed.");
            textBox1.AppendText($"{Environment.NewLine}If you leave @WorkflowId = -1, you will see the latest workflow status.");
            textBox1.AppendText($"{Environment.NewLine}Otherwise you can change the parameter to an actual WorkflowId that ran before...");
            textBox1.AppendText($"{Environment.NewLine}");
            textBox1.AppendText($"{Environment.NewLine}EXEC [WorkflowCore].[dbo].[GetWorkflowStatus] @WorkflowId = -1");
            textBox1.AppendText($"{Environment.NewLine}GO");
        }
    }
}
