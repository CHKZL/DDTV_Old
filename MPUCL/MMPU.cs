using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace MPUCL
{
    public class MMPU
    {
        public const string RoomConfigFile = "./RoomListConfig.json";
        public static List<HashSet<string>> DMlist = new List<HashSet<string>>();
        public static List<Room.RoomCadr> RoomConfigList = new List<Room.RoomCadr>();
        public static List<Room.RoomInfo> RoomInfo = new List<Room.RoomInfo>();

        public static bool IsRefreshed = false;
        // TODO: Merge List
        public static List<int> DmNum = new List<int>();
        public static bool DanmakuEnabled = false;

        public static string getDanmaku(string room, int R)
        {
            string postString = "roomid=" + room + "&token=&csrf_token=";
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            string url = @"http://api.live.bilibili.com/ajax/msg";

            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("Cookie", "");
            byte[] responseData = webClient.UploadData(url, "POST", postData);
            string srcString = Encoding.UTF8.GetString(responseData); 
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(srcString);
                for (int i = 0; i < 10; i++)
                {
                    string dmkText = jo["data"]["room"][i]["text"].ToString();
                    DMlist[R].Add(dmkText);
                }
            }
            catch (Exception)
            {
                // Ignored
            }
            return srcString;
        }

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
                if (objPinReply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    objPinReply = objPingSender.Send("223.6.6.6", intTimeout, buffer, objPinOptions);
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
            if(!File.Exists(file))
                File.Move("./RoomListConfig.ini", file);
            return File.ReadAllText(file);
        }

        public static string GetUrlContent(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                using (Stream stream = resp.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            return result;
        }

        public static string HttpDownloadFile(string url, string path)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (Stream outputStream = new FileStream(path, FileMode.Create))
                        {
                            responseStream.CopyTo(outputStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "0";
            }
            return path;
        }

        public static void DownloadBilibili(string target,string title)
        {
            Thread T1 = new Thread(new ThreadStart(delegate {
                int retry = 0;

                while (true)
                {
                    if (HttpDownloadFile(getUriSteam.GetTrueUrl(target), $"./tmp/{target}_{title}{DateTime.Now.ToString("yyyyMMddHHmmss")}.flv") != "0")
                    {
                        retry = 0;
                    }

                    retry++;

                    if (retry >= 10)
                    {
                        break;
                    }

                    Thread.Sleep(10000);
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }

        public static void InitializeRoomList()
        {
            var rlc = new Room.RoomBox();
            rlc = JsonConvert.DeserializeObject<Room.RoomBox>(ReadConfigFile(RoomConfigFile));
            RoomConfigList = rlc?.data;

            if (RoomConfigList == null)
                RoomConfigList = new List<Room.RoomCadr>();
        }

        public string generateUniqueId()
        {
            var valHWID = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001", "HwProfileGuid", "");
            SHA1 sha = new SHA1CryptoServiceProvider();
            var result = sha.ComputeHash(Encoding.UTF8.GetBytes(valHWID));
            return Convert.ToBase64String(result);
        }
    }
}
