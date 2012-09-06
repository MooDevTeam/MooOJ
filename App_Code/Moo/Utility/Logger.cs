using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using Moo.DB;
using Moo.Authorization;
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
            Log(e.ToString(), 2);
        }
        public static void Log(string message, int frameID = 1)
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

        public static User GetCurrentUser(MooDB db)
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
            {
                IIdentity identity = (IIdentity)HttpContext.Current.User.Identity;
                User currentUser = null;
                if (identity is SiteUser)
                {
                    currentUser = ((SiteUser)identity).GetDBUser(db);
                }
                return currentUser;
            }
            else
            {
                return null;
            }
        }

        public static string GetRemoteAddress()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UserHostAddress != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return "";
            }
        }

        public static void Debug(MooDB db, string info)
        {
            db.Logs.AddObject(new Log()
            {
                CreateTime = DateTimeOffset.Now,
                Level = (byte)LogLevel.Debug,
                User = GetCurrentUser(db),
                Info = info,
                RemoteAddress = GetRemoteAddress()
            });
            db.SaveChanges();
        }

        public static void Info(MooDB db, string info)
        {
            db.Logs.AddObject(new Log()
            {
                CreateTime = DateTimeOffset.Now,
                Level = (byte)LogLevel.Info,
                User = GetCurrentUser(db),
                Info = info,
                RemoteAddress = GetRemoteAddress()
            });
            db.SaveChanges();
        }

        public static void Warning(MooDB db, string info)
        {
            db.Logs.AddObject(new Log()
            {
                CreateTime = DateTimeOffset.Now,
                Level = (byte)LogLevel.Warning,
                User = GetCurrentUser(db),
                Info = info,
                RemoteAddress = GetRemoteAddress()
            });
            db.SaveChanges();
        }

        public static void Error(MooDB db, string info)
        {
            db.Logs.AddObject(new Log()
            {
                CreateTime = DateTimeOffset.Now,
                Level = (byte)LogLevel.Error,
                User = GetCurrentUser(db),
                Info = info,
                RemoteAddress = GetRemoteAddress()
            });
            db.SaveChanges();
        }

        public static void Fatal(MooDB db, string info)
        {
            db.Logs.AddObject(new Log()
            {
                CreateTime = DateTimeOffset.Now,
                Level = (byte)LogLevel.Fatal,
                User = GetCurrentUser(db),
                Info = info,
                RemoteAddress = GetRemoteAddress()
            });
            db.SaveChanges();
        }
    }
}