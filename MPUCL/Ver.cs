using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MPUCL
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class Ver
    {
        public const string VER = "1.5.1";
        public const string DATE = "(2019-3-1)";
        public const string DESC = "修改API";
        public static readonly string OS_VER = $"({SystemVer.Major}.{SystemVer.Minor}.{SystemVer.Build})";
        public static readonly string UA = $"FeelyBlog/1.1 (zyzsdy@foxmail.com) BiliRoku/1.5.1 {OS_VER} AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.119 Safari/537.36";

        public static Version Version => Version.Parse(VER);

        private static readonly Version SystemVer = Environment.OSVersion.Version;
    }

    // 检查更新
    public delegate void InfoEvent(object sender, string info);
    public delegate void CheckResultEvent(object sender, UpdateResultArgs result);

    public class UpdateResultArgs
    {
        public string version;
        public string url;
    }

    class CheckUpdate
    {
        public event InfoEvent OnInfo;
        public event CheckResultEvent OnResult;
        public CheckUpdate()
        {
            Check();
        }

        private void Check()
        {
            Task.Run(() =>
            {
                OnInfo?.Invoke(this, "检查更新。");

                var ApiUrl = "https://api.github.com/repos/zyzsdy/biliroku/releases";
                var wc = new WebClient();
                wc.Headers.Add("Accept: application/json;q=0.9,*/*;q=0.5");
                wc.Headers.Add("User-Agent: " + Ver.UA);
                wc.Headers.Add("Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,ja;q=0.4");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                //发送HTTP请求获取Release信息
                string releaseJson;

                try
                {
                    var releaseByte = wc.DownloadData(ApiUrl);
                    releaseJson = Encoding.UTF8.GetString(releaseByte);
                }
                catch (Exception e)
                {
                    OnInfo?.Invoke(this, "检查更新失败：" + e.Message);
                    return;
                }

                string tag;
                string url;

                try
                {
                    var releaseObj = JArray.Parse(releaseJson);
                    var releaseNote = releaseObj[0];
                    tag = releaseNote["tag_name"].ToString();
                    url = releaseNote["html_url"].ToString();
                }
                catch (Exception e)
                {
                    OnInfo?.Invoke(this, "更新信息解析失败：" + e.Message);
                    OnInfo?.Invoke(this, releaseJson);
                    return;
                }

                Version verNew;

                if (!Version.TryParse(tag, out verNew))
                {
                    OnInfo?.Invoke(this, "版本信息无法解析。");
                    return;
                }

                if (verNew <= Ver.Version)
                {
                    OnInfo?.Invoke(this, "当前已是最新版本。");
                    return;
                }

                try
                {
                    OnResult?.Invoke(this, new UpdateResultArgs
                    {
                        version = tag,
                        url = url
                    });
                }
                catch (Exception e)
                {
                    OnInfo?.Invoke(this, "发现新版本，但是出了点罕见错误：" + e.Message);
                }
                finally
                {
                    OnInfo?.Invoke(this, "发现新版本" + tag + "，下载地址：" + url);
                }
            });
        }
    }
}
