using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
namespace Moo.Utility
{
    /// <summary>
    /// 文件工具
    /// </summary>
    public static class FileUtil
    {
        public static string ReadTextFile(string path, int maxLength)
        {
            byte[] buf=new byte[maxLength];
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                int length=stream.Read(buf, 0, maxLength);
                return Encoding.UTF8.GetString(buf, 0, length);
            }
        }

        public static string ReadBinaryFile(string path, int maxLength)
        {
            byte[] buf = new byte[maxLength];
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                int length = stream.Read(buf, 0, maxLength);
                return Encoding.UTF8.GetString(buf, 0, length);
            }
        }
    }
}