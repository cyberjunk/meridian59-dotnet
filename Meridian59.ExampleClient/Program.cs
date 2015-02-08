using System;
using System.Windows.Forms;
using System.Collections.Concurrent;
using Meridian59.Protocol;
using Meridian59.Files.RSB;
using Meridian59.Data;
using Meridian59.DebugUI;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.GameMessages;
using Meridian59.Data.Models;
using Meridian59.Files;

namespace Meridian59.ExampleClient
{
    static class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create example client and start it
            ExampleClient.Singleton.Start(true);
        }
    }
}
