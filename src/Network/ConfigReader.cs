using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Network
{
    // 文本配置读取
    public class TxtCfg
    {
        List<string[]> lineArray = new List<string[]>();

        public int GetLineCount()
        {
            return lineArray.Count;
        }
        public string[] GetLine(int index)
        {
            return lineArray[index];
        }

        public void SaveTo(string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("gb2312"));
                sw.AutoFlush = true;

                var iter = lineArray.GetEnumerator();
                while (iter.MoveNext())
                {
                    string[] colArray = iter.Current;
                    string newLine = "";
                    var colArrayLength = colArray.Length;
                    for (int j = 0; j < colArrayLength; ++j)
                    {
                        if (j == 0)
                            newLine += colArray[j];
                        else
                            newLine += "\t" + colArray[j];
                    }
                    sw.WriteLine(newLine);
                }
                sw.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("写入文件失败:{0}\n原因:{1}", fileName, ex.Message);
            }
        }

        public static TxtCfg LoadFile(string filePath)
        {
            return LoadFile(filePath, null);
        }
        public static TxtCfg LoadFile(string filePath, char[] splitChar)
        {
            try
            {
                if(splitChar == null)
                    splitChar = new char[]{ '\t', ' ' };

                TxtCfg txtCfg = new TxtCfg();
                StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312"));
                string lineString = sr.ReadLine();
                while (lineString != null)
                {
                    if (lineString.Trim().Length > 0 && lineString.Contains("//") == false)
                    {
                        txtCfg.lineArray.Add(lineString.Split(splitChar, StringSplitOptions.RemoveEmptyEntries));
                    }
                    lineString = sr.ReadLine();
                }
                sr.Close();
                return txtCfg;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("打开配置文件失败:{0}\n原因:{1}", filePath, ex.Message);
            }
            return null;
        }

        public static TxtCfg LoadBuff(string fileBuffer)
        {
            return LoadBuff(fileBuffer, null);
        }
        public static TxtCfg LoadBuff(string fileBuffer, char[] splitChar)
        {
            TxtCfg txtCfg = new TxtCfg();
            StringReader sr = new StringReader(fileBuffer);
            string lineString = sr.ReadLine();

            if (splitChar == null)
                splitChar = new char[] { '\t', ' ' };

            while (lineString != null)
            {
                if (lineString.Contains("//") == false)
                {
                    txtCfg.lineArray.Add(lineString.Split(splitChar, StringSplitOptions.RemoveEmptyEntries));
                }
                lineString = sr.ReadLine();
            }
            sr.Close();
            return txtCfg;
        }
    }
}