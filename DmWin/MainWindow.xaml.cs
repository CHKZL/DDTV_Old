using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp.Wpf;
using MPUCL;


namespace DmWin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool KA = false;
        public static int R = 0;
        public MainWindow(string A)
        {
            InitializeComponent();
            R = int.Parse(A);
            if (A != "0")
            {
                // KA = true;
                List<string> cs = new List<string>() { "0"};
                MMPU.DMlist.Add(cs);
                持续读取弹幕放入弹幕list(getUriSteam.GetRoomid(A));
                Thread T1 = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        this.Dispatcher.Invoke(
                 new Action(
                   delegate
                   {
                       Browser.Width = this.Width;
                       Browser.Height = this.Height;
                   }
                   ));

                        Thread.Sleep(500);
                    }
                }));
                T1.IsBackground = true;
                T1.Start();
                // this.SizeChanged += new System.Windows.SizeChangedEventHandler(MainWindow_Resize);
                // this.Closed += MainWindow_Closed1; ;
                //Browser.Address = (@"Y:\DDTV\1.0.1.5\DM_System\index.html");

                string asdsad = AppDomain.CurrentDomain.BaseDirectory + @"index.html";
                // Browser.Address = AppDomain.CurrentDomain.BaseDirectory + @"index.html";
                Browser.Address = (asdsad);
                Browser.Margin = new Thickness(0, 0, 0, 0);
                CefSharp.CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                Browser.RegisterJsObject("boud", new JsEvent(), new CefSharp.BindingOptions() { CamelCaseJavascriptNames = false });
            }
        }

        private void MainWindow_Closed1(object sender, EventArgs e)
        {
            KA = false;
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            Browser.Width = this.Width;
            Browser.Height = this.Height;
        }
        public class JsEvent
        {
            public string js { get; set; }
            public void ShowTest()
            {
                try
                {
                    string DM = MMPU.DMlist[R][MMPU.DmNum];
                    MMPU.DmNum++;
                    js = DM;
                }
                catch (Exception ex)
                {
                    string asd = ex.ToString();
                    js = "0";
                }
            }
        }

        public void 持续读取弹幕放入弹幕list(string RoomId)
        {
            Thread T1 = new Thread(new ThreadStart(delegate {
                while (true)
                {
                    try
                    {
                        MMPU.getbalabala(RoomId, R);
                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(1000);
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
    }
}
