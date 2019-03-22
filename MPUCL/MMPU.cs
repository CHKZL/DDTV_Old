using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MPUCL
{
    public class MMPU
    {
        public static string RoomConfigFile = "./RoomListConfig.json";
        public static List<List<string>> DMlist = new List<List<string>>();
        public static List<Room.RoomCadr> RoomConfigList = new List<Room.RoomCadr>();//房间信息1List
        public static List<Room.RoomInfo> RoomInfo = new List<Room.RoomInfo>();//房间信息2List
        public static List<Room.RecordVideo> RecordInfo = new List<Room.RecordVideo>();//录制情况
        public static int RecNum = 0;
        public static int RecMax = 2;

        public static bool IsRefreshed = false;
        public static List<int> DmNum = new List<int>();
        /// <summary>
        /// 弹幕开关标志位
        /// </summary>
        public static bool DanmakuEnabled = false;
        public class danmu
        {
            public int mode { get; set; }//模式
            public string text { get; set; }//内容
            public int stime { get; set; }//时间
            public int size { get; set; }//大小
            public string color { get; set; }//颜色
        }
        /// <summary>
        /// 获取房间的弹幕
        /// </summary>
        /// <param name="room">房间号</param>
        /// <returns></returns>
        public static string getDanmaku(string room,int R)
        {
            string postString = "roomid=" + room + "&token=&csrf_token=";
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            string url = @"http://api.live.bilibili.com/ajax/msg";

            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("Cookie", "");
            byte[] responseData = webClient.UploadData(url, "POST", postData);
            string srcString = Encoding.UTF8.GetString(responseData);//解码  
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(srcString);
                for (int i = 0; i < 10; i++)
                {
                    string text = jo["data"]["room"][i]["nickname"].ToString()+"㈨"+ jo["data"]["room"][i]["text"].ToString();

                    if (!DMlist[R].Contains(text))
                    {
                        DMlist[R].Add(text);
                    }

                }
            }
            catch (Exception ex)
            {
                string A = ex.ToString();
            }
            return srcString;
        }

        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败
        /// </summary>
        /// <param name="Addr">域名或IP</param>
        /// <returns></returns>
        public static bool PingTest(string Addr = "223.5.5.5")
        {
            try
            {
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;

                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;

                Ping objPingSender = new Ping();
                PingReply objPinReply = objPingSender.Send(Addr, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (objPinReply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    objPinReply = objPingSender.Send("223.6.6.6", intTimeout, buffer, objPinOptions);
                    strInfo = objPinReply.Status.ToString();
                    return objPinReply.Status == IPStatus.Success;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void InitializeRoomConfigFile()
        {
            try
            {
                ReadConfigFile(RoomConfigFile);
            }
            catch (Exception)
            {

                File.WriteAllText(RoomConfigFile, "{}");
            }
        }
        public static string ReadConfigFile(string file)
        {
            if (!File.Exists(file))
                File.Move("./RoomListConfig.ini", file);
            return File.ReadAllText(file);
        }

        /// <summary>
        /// 通过get方式返回内容
        /// </summary>
        /// <param name="url">目标网页地址</param>
        /// <returns></returns>
        public static string GetUrlContent(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 3000;
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
        public static string HttpDownloadFile(string url, string path,string RoomId,string 标题,bool youtube)
        {
            string guid = Guid.NewGuid().ToString("N");
            string time = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
            //MMPU.RecordInfo.Add(new Room.RecordVideo { guid = guid, RoomID = RoomId, Name = 标题, File = path, Status = true, StartTime = time });
            try
            {
                WebClient _wc = new WebClientto(100000);

                _wc.Headers.Add("Accept: */*");
                _wc.Headers.Add("User-Agent: " + Ver.UA);
                _wc.Headers.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");
                _wc.DownloadProgressChanged += _wc_DownloadProgressChanged;
                if (!Directory.Exists("./tmp/" + RoomId))
                {
                    Directory.CreateDirectory("./tmp/" + RoomId);
                }
               
               
                // ReSharper restore AssignNullToNotNullAttribute
                string filena = "";
                if (youtube)
                {
                    filena = "./tmp/" + RoomId + "/" + 标题 + ".flv";
                    MMPU.RecordInfo.Add(new Room.RecordVideo { guid = guid, RoomID = RoomId, Name = 标题, File = filena, Status = true, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    filena = "./tmp/" + RoomId + "/" + 标题 + "_" + time + ".flv";
                    MMPU.RecordInfo.Add(new Room.RecordVideo { guid = guid, RoomID = RoomId, Name = 标题, File = filena, Status = true, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                RecNum++;
                _wc.DownloadFile(new Uri(url), filena);
                RecNum--;
                for (int i = 0; i < RecordInfo.Count; i++)
                {
                    if (RecordInfo[i].guid == guid)
                    {
                        RecordInfo[i].Status = false;
                        RecordInfo[i].EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
                    }
                }
                return path;
            }
            catch (Exception ex)
            {
                RecNum--;
                for (int i = 0; i < RecordInfo.Count; i++)
                {
                    if (RecordInfo[i].guid == guid)
                    {
                        RecordInfo[i].Status = false;
                        RecordInfo[i].EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
                    }
                }
                string sdsa = ex.ToString();
                return path;
            }
        }

        private static void _wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string Size = FormatSize(e.BytesReceived);
           
        }
        private static string FormatSize(long size)
        {
            if (size <= 1024)
            {
                return size.ToString("F2") + "B";
            }
            if (size <= 1048576)
            {
                return (size / 1024.0).ToString("F2") + "KB";
            }
            if (size <= 1073741824)
            {
                return (size / 1048576.0).ToString("F2") + "MB";
            }
            if (size <= 1099511627776)
            {
                return (size / 1073741824.0).ToString("F2") + "GB";
            }
            return (size / 1099511627776.0).ToString("F2") + "TB";
        }

        public static void DownloadBilibili(string A, string 标题, string time)
        {
            time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string Rid = getUriSteam.GetRoomid(A);
            HttpDownloadFile(getUriSteam.GetTrueUrl(Rid), "./tmp/" + getUriSteam.GetRoomid(A) + "_" + 标题 + "_" + time + ".flv", Rid, 标题,false);
        }

        /// <summary>
        /// 初始化房间列表
        /// </summary>
        public static void InitializeRoomList()
        {
            var rlc = new Room.RoomBox();
            rlc = JsonConvert.DeserializeObject<Room.RoomBox>(ReadConfigFile(RoomConfigFile));
            RoomConfigList = rlc?.data;

            if (RoomConfigList == null)
                RoomConfigList = new List<Room.RoomCadr>();

        }
        /// <summary>
        ///  获取标识 //用于区分设备统计使用人数，数据将匿名提交，不会用作其他通途
        /// </summary>
        /// <returns></returns>
        public static string generateUniqueId()
        {
            var valHWID = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001", "HwProfileGuid", "");
            return valHWID;
        }

    }
}
