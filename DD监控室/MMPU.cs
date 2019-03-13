using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DD监控室
{
    class MMPU
    {
        /// <summary>
        /// 获取房间的弹幕
        /// </summary>
        /// <param name="room">房间号</param>
        /// <returns></returns>
        static string 获取弹幕(string room)
        {
            string postString = "roomid=" + room + "&token=&csrf_token=";//要发送的数据
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
            string url = @"http://api.live.bilibili.com/ajax/msg";//地址  

            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            webClient.Headers.Add("Cookie", "");
            byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
            string srcString = Encoding.UTF8.GetString(responseData);//解码  
            return srcString;
        }

        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败
        /// </summary>
        /// <param name="A">域名或IP</param>
        /// <returns></returns>
        public static bool 测试网络(string A)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(A, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                     objPinReply = objPingSender.Send("223.6.6.6", intTimeout, buffer, objPinOptions);
                     strInfo = objPinReply.Status.ToString();
                    if (strInfo == "Success")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 储存文件
        /// </summary>
        /// <param name="file">文件完整路径</param>
        /// <param name="str">储存的文本内容</param>
        public static void SaveFile(string file, string str)
        {
            FileStream fs = new FileStream(file, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(str);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file">文件完整路径</param>
        /// <returns></returns>
        public static string ReadFile(string file)
        {
            string str;
            StreamReader sr = new StreamReader(file, true);
            str = sr.ReadLine().ToString();
            sr.Close();
            return str;
        }
        /// <summary>
        /// 通过get方式返回内容
        /// </summary>
        /// <param name="url">目标网页地址</param>
        /// <returns></returns>
        public static string get返回网页内容(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
       
    }
}
