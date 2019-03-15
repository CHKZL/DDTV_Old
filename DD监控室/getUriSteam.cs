using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DD监控室
{
    public class getUriSteam
    {
        public static int 流编号 = 0;
        //检测是否在直播
        public static string getBiliRoomId(string ID,string ty)
        {
            //读取设置
            var originalRoomId = ID;
            string _flvUrl = "";
            //准备查找下载地址
            //查找真实房间号
            string _roomid = "";
            switch (ty)
            {
                case "bilibili":
                    _roomid = GetRoomid(ID);
                    break;
                case "youtube":
                    try
                    {
                        string ASDASD = MMPU.get返回网页内容("https://www.youtube.com/channel/UCcnoKv531otgPrd3NcR0mag/live");
                        ASDASD = ASDASD.Replace("\\\"},\\\"playbackTracking\\\"", "㈨").Split('㈨')[0].Replace("\\\"hlsManifestUrl\\\":\\\"", "㈨").Split('㈨')[1].Replace("\",\\\"probeUrl\\\"", "㈨").Split('㈨')[0].Replace("\\", "");
                        _roomid = MMPU.get返回网页内容(ASDASD);
                    }
                    catch (Exception)
                    {
                        _roomid = "该房间未在直播";
                    }
                    break;
            }

          
            if (_roomid == "该房间未在直播"|| string.IsNullOrEmpty(_roomid))
            {
                return "该房间未在直播";
            }
            //查找真实下载地址
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
        /// <summary>
        /// 获取网页标题
        /// </summary>
        /// <param name="ID">房间号</param>
        /// <param name="来源">直播平台</param>
        /// <returns></returns>
        public static string GetUrlTitle(string ID,string 来源)
        {
            var roomWebPageUrl = "https://live.bilibili.com/" + ID;
            switch(来源)
            {
                case "bilibili":
                    {
                        roomWebPageUrl = "https://live.bilibili.com/" + ID;
                        break;
                    }
                case "youtube":
                    {
                        roomWebPageUrl = "https://www.youtube.com/channel/"+ID+"/live";
                        break;
                    }
                default:
                    {
                        return "获取网页标题失败";
                    }

            }
            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Accept: text/html");
            wc.Headers.Add("User-Agent: " + Ver.UA);
            wc.Headers.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");
            //发送HTTP请求获取真实房间号
            string roomHtml;
            try
            {
                roomHtml = wc.DownloadString(roomWebPageUrl);
            }
            catch (Exception)
            {   
                return "获取网页标题失败";
            }
            string 标题 = "";
            switch (来源)
            {
                case "bilibili":
                    {
                        try
                        {
                            标题 = roomHtml.Replace("</title>", "≯").Split('≯')[0].Replace("<title", "≯").Split('≯')[1].Replace("\">", "≯").Split('≯')[1];
                        }
                        catch (Exception)
                        {
                            return "获取网页标题失败";
                        }
                        
                        break;
                    }
                case "youtube":
                    {
                        
                        标题 = roomHtml.Replace("\"title\":\"", "㈨").Split('㈨')[1].Replace("\",", "㈨").Split('㈨')[0].Replace("\\","");
                        break;
                    }
                default:
                    {
                        return "获取网页标题失败";
                    }

            }
            
            try
            {
                return 标题.Replace("- 哔哩哔哩直播，二次元弹幕直播平台", "");
            }
            catch (Exception)
            {

                return ID;
            }

        }
        public static string GetRoomid(string ID)
        {
            var roomWebPageUrl = "https://api.live.bilibili.com/room/v1/Room/room_init?id=" + ID;
            var wc = new WebClient();
            wc.Headers.Add("Accept: text/html");
            wc.Headers.Add("User-Agent: " + Ver.UA);
            wc.Headers.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");
            //发送HTTP请求获取真实房间号
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
            //从返回结果中提取真实房间号
            try
            {
                var result = JObject.Parse(roomHtml);
                var live_status = result["data"]["live_status"].ToString();
                if(live_status!="1")
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
          
            //访问API获取结果
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

            //解析结果
            try
            {
                var jsonResult = JObject.Parse(resultString);
                var trueUrl = jsonResult["data"]["durl"][流编号]["url"].ToString();
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
