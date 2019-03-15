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
        public MainWindow(string A)
        {
            InitializeComponent();
            if(A!="0")
            {
                KA = true;
                持续读取弹幕放入弹幕list(getUriSteam.GetRoomid(A));
                this.SizeChanged += new System.Windows.SizeChangedEventHandler(MainWindow_Resize);
                this.Closed += MainWindow_Closed1; ;
                Browser.Address = (@"Y:\BILIBILI\Debug\弹幕测试\index.html");
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
           if (KA)
            {
                Browser.Width = this.Width;
                Browser.Height = this.Height;
            }
        }
        public class JsEvent
        {
            public string js { get; set; }
            public void ShowTest()
            {
                try
                {
                    string DM = MMPU.DMlist[MMPU.DmNum];
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
                    MMPU.getbalabala(RoomId);
                    Thread.Sleep(1000);
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
    }
}
