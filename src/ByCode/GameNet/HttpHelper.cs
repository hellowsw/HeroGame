using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace GameNet
{
    public class HttpHelper
    {
        /// <summary>  
        /// 发送Get请求  
        /// </summary>  
        /// <param name="url">地址</param>  
        /// <param name="dic">请求参数定义</param>  
        /// <returns></returns>  
        public static string Get(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            Debug.Log("url:" + builder.ToString());
            //添加参数  
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();

            //获取内容  
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
