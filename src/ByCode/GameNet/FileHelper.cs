using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnityDLL
{
    public class FileHelper
    {
        public static void Write(string filename, string content)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write(content);
            sw.Flush();
            sw.Dispose();
            sw.Close();
        }
    }
}
