using MPUCL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;


namespace DmWin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        
        //bool KA = false;
        public Window1(string A,int Fid)
        {
           
            InitializeComponent();
            if (A != "0")
            {
                List<string> cs = new List<string>();
                MMPU.DMlist.Add(cs);
                MMPU.DmNum.Add(0);
                // KA = true;

                Thread T1 = new Thread(new ThreadStart(delegate
                {
                    ReadDanmakuList(getUriSteam.GetRoomid(A), Fid);
                    while (true)
                    {
                        Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    Browser.Width = Width;
                                    Browser.Height = Height;
                                }
                            )
                        );

                        Thread.Sleep(500);
                    }
                }));
                T1.IsBackground = true;
                T1.Start();
                string path = AppDomain.CurrentDomain.BaseDirectory + @"index.html?Fid="+ Fid;
                Browser.Address = path;
                Browser.Margin = new Thickness(0, 0, 0, 0);
                CefSharp.CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                Browser.RegisterJsObject("boud", new JsEvent(), new CefSharp.BindingOptions() { CamelCaseJavascriptNames = false });
            }
        }
        //CommentCoreLibrary (//github.com/jabbany/CommentCoreLibrary) - Licensed under the MIT license
        //CommentCoreLibrary (//github.com/jabbany/CommentCoreLibrary) - Licensed under the MIT license
        //CommentCoreLibrary (//github.com/jabbany/CommentCoreLibrary) - Licensed under the MIT license

        private void MainWindow_Closed1(object sender, EventArgs e)
        {
           // KA = false;
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            //if (KA)
            {
                Browser.Width = Width;
                Browser.Height = Height;
            }
        }

        public class JsEvent
        {
            public string js { get; set; }
            public void ShowTest(string Fid)
            {
                int RA = int.Parse(Fid);
                try
                {
                    string DM = MMPU.DMlist[RA][MMPU.DmNum[RA]];
                    MMPU.DmNum[RA]++;
                    js = DM;
                }
                catch (Exception ex)
                {
                    js = "0";
                }
            }
        }

        public void ReadDanmakuList(string RoomId,int Fid)
        {
            Thread T1 = new Thread(new ThreadStart(delegate {
                while (true)
                {
                    try
                    {
                        MMPU.getDanmaku(RoomId, Fid);
                    }
                    catch (Exception ex)
                    {

                    }
                    Thread.Sleep(1500);
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
    }
}
