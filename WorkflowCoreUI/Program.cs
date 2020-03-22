using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkflowCoreUI
{
    static class Program
    {
        //public static FConversation fWkflow = null;
        public static FEventSample fWkflow = null;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //fWkflow = new FConversation();
            fWkflow = new FEventSample();
            Application.Run(fWkflow);
        }
    }
}
