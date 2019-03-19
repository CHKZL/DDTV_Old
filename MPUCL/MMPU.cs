using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
        public static List<Room.RoomCadr> Roomlist = new List<Room.RoomCadr>();//房间信息1List
        public static List<Room.RoomInfo> RInfo = new List<Room.RoomInfo>();//房间信息2List

        public static bool 房间列表刷新标志 = false;
        public static List<int> DmNum = new List<int>();
        public static bool 弹幕开关 = false;
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
        public static string getbalabala(string room,int R)
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
                    string A = jo["data"]["room"][i]["text"].ToString();
                    if (!DMlist[R].Contains(A))
                    {

                        DMlist[R].Add(A);
                    }
                }
            }
            catch (Exception)
            {

            }
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
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(str);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        public static void InitializeRoomConfigFile()
        {
            try
            {
                ReadFile(RoomConfigFile);
            }
            catch (Exception)
            {

                SaveFile(RoomConfigFile, "{}");
            }
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file">文件完整路径</param>
        /// <returns></returns>
        public static string ReadFile(string file)
        {
            try
            {
                StreamReader sr = new StreamReader(file, Encoding.UTF8);
                String line;
                string A1 = "";
                while ((line = sr.ReadLine()) != null)
                {
                    A1 += line.ToString();
                }
                sr.Close();
                sr.Dispose();
                return A1;
            }
            catch (Exception)
            {
                File.Move("./RoomListConfig.ini", file);
                StreamReader sr = new StreamReader(file, Encoding.UTF8);
                String line;
                string A1 = "";
                while ((line = sr.ReadLine()) != null)
                {
                    A1 += line.ToString();
                }
                sr.Close();
                sr.Dispose();
                return A1;
            }

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
        public static string HttpDownloadFile(string url, string path)
        {
            try
            {
              
                    // 设置参数
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    //发送请求并获取相应回应数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    request.Timeout = 5000;
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    Stream responseStream = response.GetResponseStream();
                    //创建本地文件写入流
                    Stream stream = new FileStream(path, FileMode.Create);
                    byte[] bArr = new byte[1024];
                    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        stream.Write(bArr, 0, size);
                        size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    stream.Close();
                    responseStream.Close();
              
            }
            catch (Exception ex)
            {
                string AASD = ex.ToString();
                return "0";
            }
            return path;
        }
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static void IsExistDirectory(string directoryPath)
        {
            if (false == System.IO.Directory.Exists(directoryPath))
            {
                //创建pic文件夹
                System.IO.Directory.CreateDirectory(directoryPath);
            }
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="srcPath"></param>
        public static void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        //如果 使用了 streamreader 在删除前 必须先关闭流 ，否则无法删除 sr.close();
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        public static void DelFile(string Path)
        {
            try
            {
                File.Delete(Path);
            }
            catch (Exception)
            {

                
            }
        }
        public static void bilibiliDlo(string A,string 标题)
        {
            Thread T1 = new Thread(new ThreadStart(delegate {
                int tt2 = 0;

                while (true)
                {
                    if (MMPU.HttpDownloadFile(getUriSteam.GetTrueUrl(A), "./tmp/" + A + "_" + 标题 + DateTime.Now.ToString("yyyyMMddHHmmss") + ".flv") != "0")
                    {
                        tt2 = 0;
                    }
                    tt2++;
                    if (tt2 >= 10)
                    {
                        break;
                    }
                    Thread.Sleep(10000); ;
                }
            }));
            T1.IsBackground = true;
            T1.Start();

            //BackMessage("目标流返回的数据已经接受完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生,录像文件:" + "./tmp/" + Roomlist[i].RoomNumber + "_" + 标题 + ".flv", Roomlist[i].Name + " 结束录制", 10000);
        }
        /// <summary>
        /// 初始化房间列表
        /// </summary>
        public static void InitializeRoomList()
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(MMPU.ReadFile(MMPU.RoomConfigFile));
            try
            {
                while (true)
                {
                    int a1015 = 0, a1014 = 0;
                    try
                    {
                        Roomlist = new List<Room.RoomCadr>();
                        //1.0.1.5兼容
                        for (int i = 0; ; i++)
                        {


                            if (jo["data"][i]["Types"].ToString() != "youtube")
                            {
                                Roomlist.Add(new Room.RoomCadr() { Name = jo["data"][i]["Name"].ToString(), RoomNumber = jo["data"][i]["RoomNumber"].ToString(), status = false, Types = jo["data"][i]["Types"].ToString(), VideoStatus = ((jo["data"][i]["VideoStatus"].ToString() == "True") ? true : false) });
                            }

                            a1015++;

                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            //1.0.1.4兼容
                            if (a1015 == 0)
                            {

                                for (int i = 0; ; i++)
                                {

                                    Roomlist.Add(new Room.RoomCadr() { Name = jo["data"][i]["Name"].ToString(), RoomNumber = jo["data"][i]["RoomNumber"].ToString(), status = false, Types = jo["data"][i]["Types"].ToString() });


                                    a1014++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            //1.0.1.3兼容
                            if (a1014 == 0)
                            {

                                for (int i = 0; ; i++)
                                {
                                    Roomlist.Add(new Room.RoomCadr() { Name = jo["data"][i]["Name"].ToString(), RoomNumber = jo["data"][i]["RoomNumber"].ToString(), status = ((jo["data"][i]["Ty"].ToString() == "True") ? true : false), Types = "bilibili" });
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string A = ex.ToString();
            }

        }
        /// <summary> 
        /// 获取MAC地址(返回第一个物理以太网卡的mac地址) //用于区分设备统计使用人数，数据将匿名提交，不会用作其他通途
        /// </summary> 
        /// <returns>成功返回mac地址，失败返回null</returns> 
        public string getMacAddress()
        {
            string macAddress = null;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet")) //是以太网卡
                    {
                        string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                        RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                        if (rk != null)
                        {
                            // 区分 PnpInstanceID     
                            // 如果前面有 PCI 就是本机的真实网卡    
                            // MediaSubType 为 01 则是常见网卡，02为无线网卡。    
                            string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                            int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                            if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI") //是物理网卡
                            {
                                macAddress = adapter.GetPhysicalAddress().ToString();
                                break;
                            }
                            else if (fMediaSubType == 1) //虚拟网卡
                                continue;
                            else if (fMediaSubType == 2) //无线网卡(上面判断Ethernet时已经排除了)
                                continue;
                        }
                    }
                }
            }
            catch
            {
                macAddress = null;
            }
            return macAddress;
        }

    }
}
