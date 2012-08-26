using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
namespace Moo.Utility
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        //public const string LOG_FILE = "D:\\MooError.log";
        public static void Log(Exception e)
        {
            Log(e.ToString(),2);
        }
        public static void Log(string message,int frameID=1)
        {
            StackTrace stack = new StackTrace(true);

            string logItem = "<logItem>\n"
                //+ "<time>" + DateTimeOffset.Now + "</time>\n"
                + "<position>" + stack.GetFrame(frameID).ToString() + "</position>\n"
                + "<message>\n" + message + "\n</message>\n"
                + "</logItem>\n";

            EventLog log = new EventLog("Moo");
            log.Source = "Moo";
            log.WriteEntry(logItem, EventLogEntryType.Error);
        }
    }
}