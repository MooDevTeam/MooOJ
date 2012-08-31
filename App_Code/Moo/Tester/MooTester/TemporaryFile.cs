using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace Moo.Tester.MooTester
{
    /// <summary>
    /// 临时性文件
    /// </summary>
    public class TemporaryFile : IDisposable
    {
        public string Path { get; set; }
        public void Dispose()
        {
            File.Delete(Path);
        }
    }
}