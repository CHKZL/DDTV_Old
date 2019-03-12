using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;
using System.Web;
using System.Threading;
using System.Net;
using static DD监控室.Room;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace DD监控室
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        public int 当前选择的直播流 = 0;
        public int 直播流的数量 = 0;
        public bool 初次启动判断 = true;
        public Size 播放窗体默认大小 = new Size(720,440);
        public int 选中的房间目录 = 0;
        public string ver = "";

        public Point 界面左上角坐标 = new Point(3, 3);


        List<VLCPlayer> VLC = new List<VLCPlayer>();
        List<PictureBox> PBOX = new List<PictureBox>();
        List<Form> FM = new List<Form>();
        List<Size> FSize = new List<Size>();
        RoomBox RB = new RoomBox();
        List<RoomCadr> Roomlist = new List<RoomCadr>();
        List<RoomInfo> RInfo = new List<RoomInfo>();


        public string 最近一个操作的房间号 = "";

       

        private void Form1_Load(object sender, EventArgs e)
        {
            检查新版本();
            彩蛋();
            读取房间配置信息并加载到内存();
            定时刷新房间状态();
            初始VLC();
            直播流的数量++;
        }

        private void 检查新版本()
        {
            Thread T1 = new Thread(new ThreadStart(delegate {
                try
                {
                    if (MMPU.测试网络("223.5.5.5"))
                    {
                        ver = MMPU.读取文件("./config.ini");
                        string 服务器版本 = MMPU.get返回网页内容("https://github.com/CHKZL/DDTV/raw/master/src/Ver.ini");
                        if (ver != 服务器版本)
                        {
                            DialogResult dr = MessageBox.Show("====有新版本可以更新====\n\n最新版本:" + 服务器版本 + "\n本地版本:" + ver + "\n\n点击确定转到最新版本网站", "有新版本", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                System.Diagnostics.Process.Start("https://github.com/CHKZL/DDTV");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
        private void 彩蛋()
        {
            Thread T2 = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    int nowHour = DateTime.Now.Hour;
                    nowHour = nowHour * 3600;
                    int nowMinute = DateTime.Now.Minute;
                    nowMinute = nowMinute * 60;
                    int 现在的时间 = nowHour + nowMinute;

                    if (现在的时间 > 0 && 现在的时间 < 3600)
                    {
                        this.Text = "午夜DD";
                    }
                    else if (现在的时间 > 21600 && 现在的时间 < 30600)
                    {
                        this.Text = "朝D天下";
                    }
                    else if (现在的时间 > 36000 && 现在的时间 < 41400)
                    {
                        this.Text = "DD直播间";
                    }
                    else if (现在的时间 > 43200 && 现在的时间 < 45000)
                    {
                        this.Text = "DD三十分";
                    }
                    else if (现在的时间 > 45900 && 现在的时间 < 48600)
                    {
                        this.Text = "今日说D";
                    }
                    else if (现在的时间 > 50400 && 现在的时间 < 54000)
                    {
                        this.Text = "聚焦三D";
                    }
                    else if (现在的时间 > 57600 && 现在的时间 < 61200)
                    {
                        this.Text = "走近D学";
                    }
                    else if (现在的时间 > 68400 && 现在的时间 < 70200)
                    {
                        this.Text = "DD联播";
                    }
                    else
                    {
                        this.Text = "DD导播中心";
                    }
                    Thread.Sleep(5000);
                }
            }));
            T2.IsBackground = true;
            T2.Start();
        }

        private void 定时刷新房间状态()
        {
            Thread T1 = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    Thread.Sleep(600000);
                    更新房间列表();
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            VLC[当前选择的直播流].SetVolume(trackBar1.Value);
            // Console.WriteLine(trackBar1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            更新流到选择的窗口();
        }
        private void 更新流到选择的窗口()
        {
            当前选择的直播流 = int.Parse(选择直播.Text);
            try
            {
                VLC[当前选择的直播流].Play(T1.Text, PBOX[当前选择的直播流].Handle);//播放 参数1rtsp地址，参数2 窗体句柄  
            }
            catch (Exception)
            {
                MessageBox.Show("该DD不存在");
                选择直播.Text = "";
            }
        }

        private void 选择直播_SelectedIndexChanged(object sender, EventArgs e)
        {
            当前选择的直播流 = int.Parse(选择直播.Text);
            try
            {
                trackBar1.Value = VLC[当前选择的直播流].GetVolume();
            }
            catch (Exception)
            {
            }
        }
        public void 初始VLC()
        {
            Form A = new Form();
            A.Size = new Size(720, 480);
            A.Text = (当前选择的直播流).ToString();
            A.Name = (当前选择的直播流).ToString();
            A.Icon = new Icon("./DDTV.ico");
            FSize.Add(new Size(720, 480));
            FM.Add(A);
            PictureBox p = new PictureBox();//实例化一个照片框
            p.BackColor = Color.Black;
            FM[0].Controls.Add(p);//添加到当前窗口集合中
            p.Size = new Size(A.Width - 界面左上角坐标.X - 20, A.Height - 界面左上角坐标.Y - 42);
            p.Location = 界面左上角坐标;
            PBOX.Add(p);
            RInfo.Add(new RoomInfo { Name = "初始化", RoomNumber = "-1", steam = "初始化", Ty = false });

            VLCPlayer vlcPlayer = new VLCPlayer();
            VLC.Add(vlcPlayer);
            A.ResizeEnd += A_ResizeEnd;
        }

        /// <summary>
        /// 创建播放列表，新建一个播放窗口
        /// </summary>
        /// <param name="标题"></param>
        public void 新建一个直播监听(string 标题)
        {
            直播流的数量++;

            选择直播.Items.Remove("添加一个新监控");
            选择直播.Items.Add((直播流的数量 - 1).ToString());
            //选择直播.Items.Add("添加一个新监控");
            当前选择的直播流 = 直播流的数量;
            int TS1 = 当前选择的直播流;
            int TS2 = TS1 - 1;
            选择直播.Text = (TS2).ToString();

            {

                Form A = new Form();
                A.Size = 播放窗体默认大小;
                A.Text = "DDTV-" + (当前选择的直播流).ToString() + " 当前直播的节目是：" + 标题;
                A.Icon = new Icon("./DDTV.ico");
                A.Show();
                A.Name = (当前选择的直播流).ToString();
                A.ResizeEnd += A_ResizeEnd;
                A.Activated += A_Activated;
                A.FormClosing += A_FormClosing;
                A.Move += A_Move;
                A.MouseWheel += A_MouseWheel; ;

                FM.Add(A);

                FSize.Add(播放窗体默认大小);

                PictureBox p = new PictureBox();//实例化一个照片框
                p.BackColor = Color.Black;
                FM[当前选择的直播流].Controls.Add(p);//添加到当前窗口集合中
                p.Size = new Size(A.Width - 界面左上角坐标.X - 20, A.Height - 界面左上角坐标.Y - 42);
                p.Location = 界面左上角坐标;
                PBOX.Add(p);

                VLCPlayer vlcPlayer = new VLCPlayer();
                VLC.Add(vlcPlayer);
            }
        }
        /// <summary>
        /// 鼠标滚轮事件(修改音量)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_MouseWheel(object sender, MouseEventArgs e)
        {
            Form F1 = sender as Form;
            当前选择的直播流 = int.Parse(F1.Name);
            选择直播.Text = F1.Name;
            float addsd = 0.0f;
            if (e.Delta > 0)
                addsd -= 0.1f;
            else
                addsd += 0.1f;
            if (addsd >= 3)
                addsd = 3;
            if (addsd <= 1)
                addsd = 1f;

            int 当前音量 = VLC[当前选择的直播流].GetVolume();

            if (e.Delta > 0)//上
            {
                当前音量 += 5;
            }
            else if (e.Delta < 0)//下
            {
                当前音量 -= 5;
            }
            if(当前音量>100)
            {
                当前音量 = 100;
            }
            else if(当前音量<0)
            {
                当前音量 = 0;
            }
            VLC[当前选择的直播流].SetVolume(当前音量);
            trackBar1.Value = 当前音量;
        }
        /// <summary>
        /// 动态窗口移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_Move(object sender, EventArgs e)
        {
            更新窗体内播放器大小(sender);
        }

        /// <summary>
        /// 动态窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form F1 = sender as Form;
            int 标号 = int.Parse(F1.Name);
            VLC[标号].Stop();
            选择直播.Items.Remove(标号.ToString());
            RInfo[标号].Ty = false;
        }

        /// <summary>
        /// 动态窗口缩放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_ResizeEnd(object sender, EventArgs e)
        {
            更新窗体内播放器大小(sender);
        }

        /// <summary>
        /// 动态窗口焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_Activated(object sender, EventArgs e)
        {
            Form A = sender as Form;
            当前选择的直播流 = int.Parse(A.Name);
            选择直播.Text = (当前选择的直播流).ToString();
        }
        private void 更新窗体内播放器大小(object sender)
        {
            Form F1 = sender as Form;
            当前选择的直播流 = int.Parse(F1.Name);
            更新控件大小();
            FSize[int.Parse(F1.Name)] = F1.Size;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VLC[当前选择的直播流].Stop();//停止播放
        }
        private void button3_Click(object sender, EventArgs e)
        {
            VLC[当前选择的直播流].SnapShot("./img/");//抓图，参数1为存储路径
        }
        public void 更新排版()
        {
            switch (PBOX.Count)
            {
                case 1:
                    PBOX[0].Location = 界面左上角坐标;
                    PBOX[0].Size = new Size(this.Width - 界面左上角坐标.X - 2, this.Height - 界面左上角坐标.Y - 2);

                    break;
                case 2:
                    PBOX[0].Location = 界面左上角坐标;
                    PBOX[0].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, this.Height - 界面左上角坐标.Y - 2);
                    PBOX[1].Location = new Point(0 + ((this.Width - 界面左上角坐标.X - 2) / 2), 界面左上角坐标.Y);
                    PBOX[1].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, this.Height - 界面左上角坐标.Y - 2);
                    break;
                case 3:
                    PBOX[0].Location = 界面左上角坐标;
                    PBOX[0].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, (this.Height - 界面左上角坐标.Y - 2) / 2);
                    PBOX[1].Location = new Point(0 + ((this.Width - 界面左上角坐标.X - 2) / 2), 界面左上角坐标.Y);
                    PBOX[1].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, (this.Height - 界面左上角坐标.Y - 2) / 2);


                    PBOX[2].Location = new Point(界面左上角坐标.X, (this.Height + 界面左上角坐标.Y - 2) / 2);
                    PBOX[2].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, (this.Height - 界面左上角坐标.Y - 2) / 2);

                    break;
                case 4:
                    PBOX[0].Location = 界面左上角坐标;
                    PBOX[0].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, (this.Height - 界面左上角坐标.Y - 2) / 2);
                    PBOX[1].Location = new Point(0 + ((this.Width - 界面左上角坐标.X - 2) / 2), 界面左上角坐标.Y);
                    PBOX[1].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, (this.Height - 界面左上角坐标.Y - 2) / 2);
                    PBOX[2].Location = new Point(界面左上角坐标.X, (this.Height + 界面左上角坐标.Y - 2) / 2);
                    PBOX[2].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, (this.Height - 界面左上角坐标.Y - 2) / 2);
                    PBOX[3].Location = new Point((this.Width + 界面左上角坐标.X - 2) / 2, (this.Height + 界面左上角坐标.Y - 2) / 2);
                    PBOX[3].Size = new Size((this.Width - 界面左上角坐标.X - 2) / 2, (this.Height - 界面左上角坐标.Y - 2) / 2);
                    break;
            }
        }
        public void 更新控件大小()
        {
            PBOX[当前选择的直播流].Size = new Size(FM[当前选择的直播流].Width - 界面左上角坐标.X - 20, FM[当前选择的直播流].Height - 界面左上角坐标.Y - 42);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            更新排版();
        }
        /// <summary>
        /// 获取B站直播流并开始监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            打开新的直播窗体(biliRoomId.Text);
        }
        private void 打开新的直播窗体(string roomId)
        {
            最近一个操作的房间号 = roomId;
            string 视频标题 = getUriSteam.获取网页标题(roomId);
            string steamdata = getUriSteam.getBiliRoomId(roomId);
            T1.Text = steamdata;
            新建一个直播监听(视频标题);
            当前选择的直播流 = int.Parse(选择直播.Text);
            VLC[当前选择的直播流].Play(steamdata, PBOX[当前选择的直播流].Handle);//播放 参数1rtsp地址，参数2 窗体句柄  
            RInfo.Add(new RoomInfo { Name = 视频标题, RoomNumber = roomId, steam = steamdata, Ty = true });
            if(VLC[当前选择的直播流].获取播放状态()==-10)
            {
                更新直播窗体(当前选择的直播流,roomId);
            }
        }
        public void 更新直播窗体(int 窗口编号, string RoomId)
        {
            FM[窗口编号].Close();
            打开新的直播窗体(RoomId);
            //DialogResult result = MessageBox.Show("为房间" + RoomId + "创建多媒体窗口时解码器返回错误，这个问题是由解码器产生的\n\n窗口创建需要用户手动操作，再次开始监听房间", "解码器内部错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("感谢以下有能man和dd:\n使用了zyzsdy 提供的 biliroku中获取web相关信息的库");
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 房间列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                打开新的直播窗体(Roomlist[index].RoomNumber);
            }
        }
        
        private int 流选择()
        {
            switch(流.Text)
            {
                case "主线":
                    return 0;
                case "备线1":
                    return 1;
                case "备线2":
                    return 2;
                case "备线3":
                    return 3;
            }
            return 0;
        }
        /// <summary>
        /// 增加房间按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RoomNametext.Text))
            {
                if (!string.IsNullOrEmpty(biliRoomId.Text))
                {
                    Roomlist.Add(new RoomCadr() { Name = RoomNametext.Text, RoomNumber = biliRoomId.Text, Ty = false });
                    更新房间列表();
                    储存房间信息到硬盘();
                }
                else
                {
                    MessageBox.Show("请输入房间号");
                }
            }
            else
            {
                MessageBox.Show("请输入房间名/备注");
            }
        }
        private void 更新房间列表()
        {
            Thread T1 = new Thread(new ThreadStart(delegate
            {
                listBox.Items.Clear();
                for (int i = 0; i < Roomlist.Count; i++)
                {
                    if (getUriSteam.getBiliRoomId(Roomlist[i].RoomNumber) == "该房间未在直播")
                    {

                        Roomlist[i].Ty = false;

                        listBox.Items.Add("[摸鱼中]○ " + Roomlist[i].Name + "：" + Roomlist[i].RoomNumber);
                    }
                    else
                    {
                        if (!Roomlist[i].Ty)
                        {
                            //设置后台气泡提示
                            string 标题 = getUriSteam.获取网页标题(Roomlist[i].RoomNumber);
                            if (!初次启动判断)
                            {
                                DDTV.ShowBalloonTip(3000, Roomlist[i].Name + " 开始直播了", 标题, ToolTipIcon.Info);
                            }
                            Roomlist[i].Ty = !Roomlist[i].Ty;
                        }
                        listBox.Items.Add("[直播中]● " + Roomlist[i].Name + "：" + Roomlist[i].RoomNumber);
                    }
                }
                初次启动判断 = false;
            }));
            T1.IsBackground = true;
            T1.Start();
        }

        public void 储存房间信息到硬盘()
        {
            RB.data = Roomlist;
            MMPU.储存文件("./RoomListConfig.ini", JsonConvert.SerializeObject(RB));
        }

        private void 读取房间配置信息并加载到内存()
        {       
            JObject jo = (JObject)JsonConvert.DeserializeObject(MMPU.读取文件("./RoomListConfig.ini"));
            try
            {
                while (true)
                {
                    for (int i = 0; ; i++)
                    {
                        Roomlist.Add(new RoomCadr() { Name = jo["data"][i]["Name"].ToString(), RoomNumber = jo["data"][i]["RoomNumber"].ToString() });
                    }
                }
            }
            catch (Exception)
            {
            }
            更新房间列表();
        }
       
        /// <summary>
        /// 单机房间列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                选中的房间目录 = index;
            }
        }

        /// <summary>
        /// 删除房间按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click_1(object sender, EventArgs e)
        {
            listBox.Items.Remove(listBox.Items[选中的房间目录]);
            Roomlist.RemoveAt(选中的房间目录);
            更新房间列表();
            储存房间信息到硬盘();
        }

        /// <summary>
        /// 刷新房间按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            更新房间列表();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
        private string 更新并获取直播流(string RID)
        {
            string steam = getUriSteam.getBiliRoomId(RID);
            RInfo[当前选择的直播流].steam = steam;
            return steam;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// 修改流属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 流_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (流.Text)
            {
                case "主线":
                    getUriSteam.流编号 = 0;
                    break;
                case "备线1":
                    getUriSteam.流编号 = 1;
                    break;
                case "备线2":
                    getUriSteam.流编号 = 2;
                    break;
                case "备线3":
                    getUriSteam.流编号 = 3;
                    break;
            }
        }

        private void 修改分辨率_Click(object sender, EventArgs e)
        {
            try
            {
                int X = int.Parse(窗口宽.Text);
                int Y = int.Parse(窗口高.Text);
                if (X <= 0||X>=1920||Y<=0||Y>=1080)
                {
                    MessageBox.Show("输入的数据过大或过小");
                    return;
                }
                for(int i=0;i<FM.Count;i++)
                {
                    Size C = new Size(X, Y);
                    FM[i].Size = C;
                    FSize[i] = C;
                    PBOX[i].Size = new Size(C.Width - 界面左上角坐标.X - 20, C.Height - 界面左上角坐标.Y - 42);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("输入的内容不是有效数字");
                return;
            }
           
        }
    }
}
