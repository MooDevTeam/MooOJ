using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Moo.Tester.MooTester
{
    /// <summary>
    ///MooTesterException
    /// </summary>
    public class MooTesterException : Exception
    {
        public TestResult Result { get; set; }
    }
}