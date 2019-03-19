using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MPUCL
{
    public class getUriSteam
    {
        public static int StreamId = 0;
        public static string getBiliRoomId(string ID, string type)
        {
            var originalRoomId = ID;
            string _flvUrl = "";
            string _roomid = "";
            switch (type)
            {
                case "bilibili":
                    _roomid = GetRoomid(ID);
                    break;
                case "youtube":
                    Regex reg = new Regex(@"\\""hlsManifestUrl\\"":\\""(.*?)(\\"",\\""probeUrl\\""|\\""},\\""playbackTracking\\"")");
                    try
                    {
                        string pageSource = MMPU.GetUrlContent("https://www.youtube.com/channel/UCcnoKv531otgPrd3NcR0mag/live");
                        string result = reg.Match(pageSource).Groups[1].Value;
                        result = Regex.Unescape(result);
                        _roomid = MMPU.GetUrlContent(result);
                        // So, here we get a m3u8 file. Not so-called "Room Id".
                    }
                    catch (Exception)
                    {
                        _roomid = "该房间未在直播";
                    }
                    break;
            }


            if (_roomid == "该房间未在直播" || string.IsNullOrEmpty(_roomid))
            {
                return "该房间未在直播";
            }
            try
            {
                _flvUrl = GetTrueUrl(_roomid);
            }
            catch
            {
                Console.WriteLine("未取得下载地址");
            }
            return _flvUrl;
        }

        public static string GetUrlTitle(string ID, string source)
        {
            var roomWebPageUrl = "https://live.bilibili.com/" + ID;
            switch (source)
            {
                case "bilibili":
                    roomWebPageUrl = "https://live.bilibili.com/" + ID;
                    break;
                case "youtube":
                    roomWebPageUrl = "https://www.youtube.com/channel/" + ID + "/live";
                    break;
                default:
                     return "获取网页标题失败";
            }

            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Accept: text/html");
            wc.Headers.Add("User-Agent: " + Ver.UA);
            wc.Headers.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");
            
            string roomHtml;
            try
            {
                roomHtml = wc.DownloadString(roomWebPageUrl);
            }
            catch (Exception)
            {
                return "获取网页标题失败";
            }

            string title = "";
            Regex reg1 = new Regex(@"""title"":""(.*?)""");
            Regex reg2 = new Regex(@"<title (.*?)>(.*?)</title>");
            switch (source)
            {
                case "bilibili":
                    string result = reg2.Match(roomHtml).Groups[2].Value;
                    if(result == "哔哩哔哩直播，二次元弹幕直播平台")
                        result = reg1.Match(roomHtml).Groups[1].Value;
                    title = result;
                    break;
                case "youtube":
                    title = reg1.Match(roomHtml).Groups[1].Value;
                    break;
                default:
                    return "获取网页标题失败";
            }

            return title.Replace(" - 哔哩哔哩直播，二次元弹幕直播平台", "");

        }

        public static string GetRoomid(string ID)
        {
            var roomWebPageUrl = "https://api.live.bilibili.com/room/v1/Room/room_init?id=" + ID;
            var wc = new WebClient();
            wc.Headers.Add("Accept: text/html");
            wc.Headers.Add("User-Agent: " + Ver.UA);
            wc.Headers.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");

            string roomHtml;
            try
            {
                roomHtml = wc.DownloadString(roomWebPageUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine("直播初始化失败：" + e.Message);
                return null;
            }

            try
            {
                var result = JObject.Parse(roomHtml);
                var live_status = result["data"]["live_status"].ToString();
                if (live_status != "1")
                {
                    return "该房间未在直播";
                }
                var roomid = result["data"]["room_id"].ToString();
                Console.WriteLine("真实房间号: " + roomid);
                return roomid;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR", "获取真实房间号失败：" + e.Message);
                return null;
            }
        }

        public static string GetTrueUrl(string roomid)
        {
            if (roomid == null)
            {
                Console.WriteLine("房间号获取错误。");
                throw new Exception("No roomid");
            }
            var apiUrl = "https://api.live.bilibili.com/room/v1/Room/playUrl?cid=" + roomid + "&otype=json&quality=0&platform=web";

            var wc = new WebClient();
            wc.Headers.Add("Accept: */*");
            wc.Headers.Add("User-Agent: " + Ver.UA);
            wc.Headers.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");

            string resultString;

            try
            {
                resultString = wc.DownloadString(apiUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine("发送解析请求失败：" + e.Message);
                throw;
            }

            try
            {
                var jsonResult = JObject.Parse(resultString);
                var trueUrl = jsonResult["data"]["durl"][StreamId]["url"].ToString();
                Console.WriteLine("地址解析成功：" + trueUrl);
                return trueUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine("视频流地址解析失败：" + e.Message);
                throw;
            }
        }

    }
}
