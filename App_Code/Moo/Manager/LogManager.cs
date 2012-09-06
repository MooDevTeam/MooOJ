using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data.SqlClient;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
namespace Moo.Manager
{
    /// <summary>
    /// Log管理
    /// </summary>
    public static class LogManager
    {
        static Thread daemonThread;

        static volatile bool shouldStop;

        public static void Start()
        {
            if (daemonThread != null)
            {
                Stop();
            }

            shouldStop = false;
            daemonThread = new Thread(new ThreadStart(ThreadMain));
            daemonThread.Start();
            using (MooDB db = new MooDB())
            {
                Logger.Info(db, "日志进程启动");
            }
        }

        public static void Stop()
        {
            shouldStop = true;
            daemonThread.Interrupt();
            daemonThread.Join();
            daemonThread = null;
            using (MooDB db = new MooDB())
            {
                Logger.Info(db, "日志进程停止");
            }
        }

        static void ThreadMain()
        {
            while (!shouldStop)
            {
                try
                {
                    Thread.Sleep(MainLoop());
                }
                catch (Exception e)
                {
                    if (!(e is ThreadInterruptedException))
                    {
                        using (MooDB db = new MooDB())
                        {
                            Logger.Fatal(db, e.ToString());
                        }
                    }
                }
            }
        }

        static int MainLoop()
        {
            using (MooDB db = new MooDB())
            {
                int count = db.ExecuteStoreCommand("DELETE FROM [dbo].[Logs] WHERE [CreateTime] < @minTime", new SqlParameter("minTime", DateTimeOffset.Now.AddMonths(-1)));

                db.SaveChanges();
                if (count > 0)
                {
                    Logger.Warning(db, "删除了" + count + "条日志");
                }
            }

            return 5 * 60 * 1000;
        }
    }
}