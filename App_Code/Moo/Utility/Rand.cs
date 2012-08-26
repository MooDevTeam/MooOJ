using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
namespace Moo.Utility
{
    /// <summary>
    ///Rand 的摘要说明
    /// </summary>
    public static class Rand
    {
        static Random rand;
        static int counter;

        public static Random RAND
        {
            get
            {
                if (rand == null || counter++ % 10000 == 0)
                {
                    rand = new Random();
                }
                return rand;
            }
        }

        public static int RandomSign()
        {
            return RAND.Next(2) * 2 - 1;
        }
    }
}