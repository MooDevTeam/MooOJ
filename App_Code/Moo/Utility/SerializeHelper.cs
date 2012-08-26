using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moo.Utility
{
    /// <summary>
    /// 序列化与反序列化
    /// </summary>
    public static class SerializeHelper
    {
        public static string Serialize(object obj)
        {
            BinaryFormatter formattter =new BinaryFormatter();
            MemoryStream stream;
            using(stream=new MemoryStream()){
                formattter.Serialize(stream,obj);

                byte[] byteArray=stream.ToArray();
                return Convert.ToBase64String(byteArray,0,byteArray.Length);
            }
        }

        public static object Deserialize(string base64)
        {
            byte[] byteArray = Convert.FromBase64String(base64);
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream;
            using (stream = new MemoryStream())
            {
                return formatter.Deserialize(stream);
            }
        }
    }
}