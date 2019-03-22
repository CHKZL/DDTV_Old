using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using DmWin;
using MPUCL;
using MaterialSkin.Controls;

namespace DD监控室
{
    public partial class Main : MaterialForm
    {
        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        public int CurrentlyStream = 0;//当前选择的直播流标示
        public int streamNumber = 0;//直播流的数量标示
        public bool startType = true;//首次启动判断
        public Size playWindowDefaultSize = new Size(720, 440);//播放窗口的默认大小
        public int indexRoom = 0;
        public string IndexRoomNum = "";
        public string ver = "1.0.2.0";
        public Point WindowTopLeft = new Point(3, 3);
        public bool YtbDloB = false;

        public bool YTB = false;//youtube测试功能

        List<VLCPlayer> VLC = new List<VLCPlayer>();//动态VLC播放器对象List
        List<PictureBox> PBOX = new List<PictureBox>();//动态pictureBox对象List
        List<Form> FM = new List<Form>();//动态Form对象List
        List<Size> FSize = new List<Size>();//Form窗体大小List
                                            //RoomBox RB = new RoomBox();//房间信息

        List<Window1> DmF = new List<Window1>();

        private void Form1_Load(object sender, EventArgs e)
        {


            MMPU.InitializeRoomConfigFile();
            CheckVersion();
            egg();
            MMPU.InitializeRoomList();
            UpdateRoomList();
            TimelyRefreshingRoomStatus();
            initializationVLC();
            streamNumber++;
            MMPU.DanmakuEnabled = true;
        }

        /// <summary>
        /// 检查版本
        /// </summary>
        private void CheckVersion()
        {
            Thread T1 = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    try
                    {
                        if (MMPU.PingTest("223.5.5.5"))
                        {
                            try
                            {
                                MMPU.GetUrlContent("http://test.touhou.ren/DDTV.ashx?code=START&as=" + MMPU.generateUniqueId());
                            }
                            catch (Exception)
                            {

                            }
                            string ServerVersion = MMPU.GetUrlContent("https://github.com/CHKZL/DDTV/raw/master/src/Ver.ini").Trim();
                            string NewVersionText = MMPU.GetUrlContent("https://github.com/CHKZL/DDTV/raw/master/src/Ver_Text.ini").Trim();
                            if (ver != ServerVersion)
                            {
                                DialogResult dr = MessageBox.Show("====有新版本可以更新====\n\n最新版本:" + ServerVersion + "\n本地版本:" + ver + "\n\n" + NewVersionText + "\n\n点击确定转到本项目github页面下载", "有新版本", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                if (dr == DialogResult.OK)
                                {
                                    System.Diagnostics.Process.Start("https://github.com/CHKZL/DDTV");
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string asd = ex.ToString();
                    }
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }

        /// <summary>
        /// 菜单
        /// </summary>
        private void egg()
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
                    Thread.Sleep(1000);
                }
            }));
            T2.IsBackground = true;
            T2.Start();
        }
        public void GetYoutube(string roomId)
        {
            int 标号 = int.Parse(MMPU.RoomInfo[CurrentlyStream].Name);
            //VLC[CurrentlyStream].Play(@".\img\630529.ts", PBOX[CurrentlyStream].Handle);
            string ASDASD = MMPU.GetUrlContent("https://www.youtube.com/channel/" + roomId + "/live");
            try
            {
                ASDASD = ASDASD.Replace("\\\"},\\\"playbackTracking\\\"", "㈨").Split('㈨')[0].Replace("\\\"hlsManifestUrl\\\":\\\"", "㈨").Split('㈨')[1].Replace("\",\\\"probeUrl\\\"", "㈨").Split('㈨')[0].Replace("\\", "");
            }
            catch (Exception)
            {
                //ClForm(标号);
                MessageBox.Show("该youtube频道未开启直播");
                return;
            }
            ASDASD = MMPU.GetUrlContent(ASDASD);
            /*
             * 3  256x144 
             * 5  426x240
             * 7  640x360
             * 9  854x480
             * 11 1280x720
             * 13 1920x1080
             */

            string[] ASD = ASDASD.Split('\n');
            bool 播放开始 = false;

            Thread T1 = new Thread(new ThreadStart(delegate
            {

                List<string> LS1 = new List<string>();
                int S = 0;
                while (true)
                {
                    if (!MMPU.RoomInfo[标号].status)
                    {
                        return;
                    }
                    if (FM[标号].Visible == false)
                    {

                        return;
                    }
                    ASDASD = MMPU.GetUrlContent(ASD[CurrentResolution()]);
                    /*
                     * 6   1
                     * 8   2
                     * 10  3
                     */
                    string[] LS3 = ASDASD.Split('\n');
                    List<string[]> LS2 = new List<string[]>();

                    for (int i = 0; i < LS3.Length; i++)
                    {
                        if (LS3[i].Length != LS3[i].Replace("http", "").Length)
                        {
                            string[] A = new string[2];

                            Regex reg = new Regex("index.m3u8/sq/(.+)/goap");
                            Match match = reg.Match(LS3[i]);
                            string value = match.Groups[1].Value;
                            A[0] = LS3[i];
                            A[1] = value;
                            LS2.Add(A);
                        }
                    }


                    Directory.CreateDirectory("./tmp/" + roomId + "/");
                    for (int i = 0; i < LS2.Count; i++)
                    {
                        LS1.Add(LS2[i][1]);
                        try
                        {
                            if (MMPU.HttpDownloadFile(LS2[i][0], "./tmp/" + roomId + "/" + LS2[i][1] + ".ts", roomId, LS2[i][1],true) != "0")
                            {
                                S++;
                            }
                        }
                        catch (Exception)
                        {
                            BackMessage("目标流返回的数据已经接受完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生", "录制结束", 10000);
                            return;
                        }

                    }
                    MMPU.RoomInfo[标号].status = true;
                    if (S > 5 && !播放开始)
                    {

                        播放开始 = true;
                        Thread T2 = new Thread(new ThreadStart(delegate
                        {
                            int S1 = int.Parse(LS1[0]);

                            FileStream fs = new FileStream("./tmp/" + roomId + "/" + S1 + ".ts", FileMode.Append);//以Append方式打开文件
                            BinaryWriter bw = new BinaryWriter(fs); //二进制写入流
                            MMPU.RoomInfo[标号].steam = "./tmp/" + roomId + "/" + S1 + ".ts";
                            //VLC[CurrentlyStream].Play("./tmp/" + roomId + "/" + S1 + ".ts", PBOX[CurrentlyStream].Handle);
                            S1++;
                            while (true)
                            {
                                if (!MMPU.RoomInfo[标号].status)
                                {
                                    fs.Close();
                                    bw.Close();
                                    return;
                                }
                                if (FM[标号].Visible == false)
                                {
                                    fs.Close();
                                    bw.Close();
                                    return;
                                }
                                try
                                {
                                    if (File.Exists("./tmp/" + roomId + "/" + (S1 + 1) + ".ts"))
                                    {
                                        FileStream fs1 = new FileStream("./tmp/" + roomId + "/" + S1 + ".ts", FileMode.Open);//以Append方式打开文件
                                        BinaryReader br1 = new BinaryReader(fs1);
                                        byte[] Q2 = br1.ReadBytes((int)br1.BaseStream.Length);
                                        bw.Write(Q2, 0, Q2.Length); //写文件

                                        fs1.Close();
                                        br1.Close();
                                        File.Delete("./tmp/" + roomId + "/" + S1 + ".ts");
                                        S1++;
                                    }
                                    else
                                    {
                                        if (File.Exists("./tmp/" + roomId + "/" + (S1 + 2) + ".ts"))
                                        {
                                            S1++;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    Thread.Sleep(10);
                                }

                            }
                        }));
                        T2.IsBackground = true;
                        T2.Start();

                    }

                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
        /// <summary>
        /// 当前youtube分辨率
        /// </summary>
        /// <returns></returns>
        private int CurrentResolution()
        {
            switch (Resolution.Text)
            {
                case "256x144":
                    return 3;
                case "426x240":
                    return 5;
                case "640x360":
                    return 7;
                case "854x480":
                    return 9;
                case "1280x720":
                    return 11;
                case "1920x1080":
                    return 13;
                default:
                    return 7;
            }
        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            VLC[CurrentlyStream].Volume = trackBar1.Value;
            EditTitleVolume(CurrentlyStream, trackBar1.Value);//修改标题音量显示
            // Console.WriteLine(trackBar1.Value);
        }
        /// <summary>
        /// 修改标题音量显示
        /// </summary>
        /// <param name="标号"></param>
        /// <param name="音量"></param>
        public void EditTitleVolume(int 标号, int 音量)
        {
            FM[标号].Text = "DDTV-" + MMPU.RoomInfo[标号].Name + " 点击标题按F5可刷新 音量:" + 音量 + "%" + " 正在直播:" + MMPU.RoomInfo[标号].Text + "  如果弹幕被挡住了，把鼠标移动到这↓下面↓的这条白线上晃动一下";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            更新流到选择的窗口();
        }
        private void 更新流到选择的窗口()
        {
            CurrentlyStream = int.Parse(liveIndex.Text);
            try
            {
                VLC[CurrentlyStream].Play(T1.Text, PBOX[CurrentlyStream].Handle);//播放 参数1rtsp地址，参数2 窗体句柄  
            }
            catch (Exception)
            {
                MessageBox.Show("该DD不存在");
                liveIndex.Text = "";
            }
        }

        private void 选择直播_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentlyStream = int.Parse(liveIndex.Text);
            try
            {
                trackBar1.Value = VLC[CurrentlyStream].Volume;
            }
            catch (Exception)
            {
            }
        }
        public void initializationVLC()
        {
            Form A = new Form();
            A.Size = new Size(720, 480);
            A.Text = (CurrentlyStream).ToString();
            A.Name = (CurrentlyStream).ToString();
            A.Icon = new Icon("./DDTV.ico");
            FSize.Add(new Size(720, 480));
            FM.Add(A);
            PictureBox p = new PictureBox();//实例化一个照片框
            p.BackColor = Color.Black;
            FM[0].Controls.Add(p);//添加到当前窗口集合中
            p.Size = new Size(A.Width - WindowTopLeft.X - 20, A.Height - WindowTopLeft.Y - 42);
            p.Location = WindowTopLeft;
            PBOX.Add(p);
            MMPU.RoomInfo.Add(new Room.RoomInfo { Name = "初始化", RoomNumber = "-1", steam = "初始化", status = false });

            VLCPlayer vlcPlayer = new VLCPlayer();
            VLC.Add(vlcPlayer);
            A.ResizeEnd += A_ResizeEnd;

            Window1 W1 = null;
            DmF.Add(W1);

            List<string> cs = new List<string>();
            MMPU.DMlist.Add(cs);

            MMPU.DmNum.Add(0);

        }

        /// <summary>
        /// 创建播放列表，新建一个播放窗口
        /// </summary>
        /// <param name="标题"></param>
        public void NewLiveWindow(string 标题)
        {
            streamNumber++;

            liveIndex.Items.Remove("添加一个新监控");
            liveIndex.Items.Add((streamNumber - 1).ToString());
            //选择直播.Items.Add("添加一个新监控");
            CurrentlyStream = streamNumber;
            int TS1 = CurrentlyStream;
            int TS2 = TS1 - 1;
            liveIndex.Text = (TS2).ToString();

            {

                Form A = new Form();
                A.Size = playWindowDefaultSize;

                //A.Text = "DDTV-" + (当前选择的直播流).ToString() + " 当前直播的节目是：" + 标题;
                A.Icon = new Icon("./DDTV.ico");
                A.Show();
                A.Name = (CurrentlyStream).ToString();
                A.KeyPreview = true;
                A.ResizeEnd += A_ResizeEnd;
                A.Activated += A_Activated;
                A.FormClosing += A_FormClosing;
                A.Move += A_Move;
                A.MouseWheel += A_MouseWheel;
                A.KeyDown += A_KeyDown;
                A.MouseMove += A_MouseMove;

                FM.Add(A);

                FSize.Add(playWindowDefaultSize);

                PictureBox p = new PictureBox();//实例化一个照片框
                p.BackColor = Color.Black;
                FM[CurrentlyStream].Controls.Add(p);//添加到当前窗口集合中
                p.Size = new Size(A.Width - WindowTopLeft.X - 20, A.Height - WindowTopLeft.Y - 42);
                p.Location = WindowTopLeft;
                PBOX.Add(p);

                VLCPlayer vlcPlayer = new VLCPlayer();
                VLC.Add(vlcPlayer);

                TopInfo.Checked = false;
            }
        }

        /// <summary>
        /// 鼠标移动到上面发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_MouseMove(object sender, EventArgs e)
        {
            Form A = sender as Form;
            CurrentlyStream = int.Parse(A.Name);
            liveIndex.Text = (CurrentlyStream).ToString();
            TopInfo.Checked = MMPU.RoomInfo[CurrentlyStream].Top;
            if (MMPU.DanmakuEnabled)
            {
                try
                {
                    DmF[CurrentlyStream].Topmost = true;
                    DmF[CurrentlyStream].Topmost = false;
                }
                catch (Exception)
                {
                }

            }

        }

        private void A_KeyDown(object sender, KeyEventArgs e)
        {
            Form F1 = sender as Form;
            if (e.KeyData == Keys.F5)
            {
                try
                {
                    int.Parse(MMPU.RoomInfo[CurrentlyStream].RoomNumber);
                    string roomId = MMPU.RoomInfo[int.Parse(F1.Name)].RoomNumber;
                    string VideoTitle = getUriSteam.GetUrlTitle(roomId, "bilibili");
                    string steamData = getUriSteam.getBiliRoomId(roomId, "bilibili");
                    T1.Text = steamData;
                    CurrentlyStream = int.Parse(F1.Name);
                    VLC[CurrentlyStream].Play(steamData, PBOX[CurrentlyStream].Handle);
                    MMPU.RoomInfo[CurrentlyStream] = (new Room.RoomInfo { Name = CurrentlyStream.ToString(), RoomNumber = roomId, steam = steamData, status = true, Text = VideoTitle });
                    EditTitleVolume(CurrentlyStream, trackBar1.Value);
                    if (VLC[CurrentlyStream].GetPlayerState() == -10)
                    {
                        UpdateLiveForm(CurrentlyStream, roomId);
                    }
                }
                catch (Exception)
                {
                    UpdateLiveForm(int.Parse(F1.Name), MMPU.RoomInfo[int.Parse(F1.Name)].RoomNumber);
                }




                Console.WriteLine("按下了F5");
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
            CurrentlyStream = int.Parse(F1.Name);
            liveIndex.Text = F1.Name;
            float addsd = 0.0f;
            if (e.Delta > 0)
                addsd -= 0.1f;
            else
                addsd += 0.1f;
            if (addsd >= 3)
                addsd = 3;
            if (addsd <= 1)
                addsd = 1f;

            int 当前音量 = VLC[CurrentlyStream].Volume;

            if (e.Delta > 0)//上
            {
                当前音量 += 5;
            }
            else if (e.Delta < 0)//下
            {
                当前音量 -= 5;
            }
            if (当前音量 > 100)
            {
                当前音量 = 100;
            }
            else if (当前音量 < 0)
            {
                当前音量 = 0;
            }
            VLC[CurrentlyStream].Volume = 当前音量;
            trackBar1.Value = 当前音量;
            EditTitleVolume(CurrentlyStream, 当前音量);
        }
        /// <summary>
        /// 动态窗口移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_Move(object sender, EventArgs e)
        {
            更新窗体内播放器大小(sender);
            if (MMPU.DanmakuEnabled)
            {
                try
                {
                    DmF[CurrentlyStream].Width = FM[CurrentlyStream].Width - 20;
                    DmF[CurrentlyStream].Height = FM[CurrentlyStream].Height - 50;

                    DmF[CurrentlyStream].Top = FM[CurrentlyStream].Top + 30;
                    DmF[CurrentlyStream].Left = FM[CurrentlyStream].Left + 10;
                    DmF[CurrentlyStream].Topmost = true;
                    DmF[CurrentlyStream].Topmost = false;
                }
                catch (Exception)
                {
                }

            }
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
            if (VLC[标号].IsInitlized)
            {
                VLC[标号].Stop();
                VLC[标号].Dispose();
            }

            liveIndex.Items.Remove(标号.ToString());
            new Thread(new ThreadStart(delegate
            {
                MMPU.RoomInfo[标号].status = false;
                Thread.Sleep(1000);
                //Directory.Delete("./tmp/" + MMPU.RoomInfo[标号].RoomNumber);
                if (MMPU.DanmakuEnabled)
                {
                    try
                    {
                        this.DmF[标号].Dispatcher.Invoke(
                  new Action(
                    delegate
                    {
                        DmF[标号].Close();
                    }
                    ));
                    }
                    catch (Exception ex)
                    {
                        string asd = ex.ToString();
                    }
                }
            })).Start();

        }

        /// <summary>
        /// 动态窗口缩放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_ResizeEnd(object sender, EventArgs e)
        {
            更新窗体内播放器大小(sender);
            if (MMPU.DanmakuEnabled)
            {
                try
                {
                    DmF[CurrentlyStream].Width = FM[CurrentlyStream].Width - 20;
                    DmF[CurrentlyStream].Height = FM[CurrentlyStream].Height - 50;

                    DmF[CurrentlyStream].Top = FM[CurrentlyStream].Top + 30;
                    DmF[CurrentlyStream].Left = FM[CurrentlyStream].Left + 10;
                    DmF[CurrentlyStream].Topmost = true;
                    DmF[CurrentlyStream].Topmost = false;

                }
                catch (Exception)
                {
                }

            }
        }

        /// <summary>
        /// 动态窗口焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_Activated(object sender, EventArgs e)
        {
            Form A = sender as Form;
            CurrentlyStream = int.Parse(A.Name);
            liveIndex.Text = (CurrentlyStream).ToString();
            TopInfo.Checked = MMPU.RoomInfo[CurrentlyStream].Top;
            if (MMPU.DanmakuEnabled)
            {
                try
                {
                    DmF[CurrentlyStream].Topmost = true;
                    DmF[CurrentlyStream].Topmost = false;
                }
                catch (Exception)
                {

                }

            }
        }
        private void 更新窗体内播放器大小(object sender)
        {
            Form F1 = sender as Form;
            CurrentlyStream = int.Parse(F1.Name);
            PBOX[CurrentlyStream].Size = new Size(FM[CurrentlyStream].Width - WindowTopLeft.X - 20, FM[CurrentlyStream].Height - WindowTopLeft.Y - 42);
            FSize[int.Parse(F1.Name)] = F1.Size;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="A">房间标号</param>
        private void ClForm(int A)
        {
            FM[A].Close();
            VLC[A].Stop();
            liveIndex.Items.Remove(A.ToString());
            new Thread(new ThreadStart(delegate
            {
                MMPU.RoomInfo[A].status = false;
                Thread.Sleep(1000);
                Directory.Delete("./tmp/" + MMPU.RoomInfo[A].RoomNumber);
            })).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VLC[CurrentlyStream].Stop();//停止播放
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // VLC[CurrentlyStream].Snapshot("./img/");//抓图，参数1为存储路径
            RecordForm RF = new RecordForm();
            RF.Show();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 获取B站直播流并开始监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            NewLive(biliRoomId.Text);
        }
        private void NewLive(string roomId)
        {
            if (!string.IsNullOrEmpty(roomId))
            {
                roomId = roomId.Replace("https://live.bilibili.com/", "");
            }
            try
            {
                int.Parse(roomId);
                string VideoTitle = "";
                try
                {
                    VideoTitle = getUriSteam.GetUrlTitle(roomId, "bilibili");
                }
                catch (Exception)
                {
                    VideoTitle = "房间名获取失败";
                }
                string steamData = getUriSteam.getBiliRoomId(roomId, "bilibili");
                T1.Text = steamData;
                if (steamData == "该房间未在直播")
                {
                    MessageBox.Show("该房间未在直播");
                    return;
                }
                NewLiveWindow(VideoTitle);
                CurrentlyStream = int.Parse(liveIndex.Text);
                VLC[CurrentlyStream].Play(steamData, PBOX[CurrentlyStream].Handle);
                MMPU.RoomInfo.Add(new Room.RoomInfo { Name = CurrentlyStream.ToString(), RoomNumber = roomId, steam = steamData, status = true, Text = VideoTitle });
                EditTitleVolume(CurrentlyStream, trackBar1.Value);
                if (VLC[CurrentlyStream].GetPlayerState() == -10)
                {

                    Window1 W1 = null;
                    DmF.Add(W1);


                    UpdateLiveForm(CurrentlyStream, roomId);
                    return;
                }
                if (MMPU.DanmakuEnabled)
                {
                    Window1 W1 = new Window1(roomId, CurrentlyStream);
                    W1.Width = FM[CurrentlyStream].Width - 20;
                    W1.Height = FM[CurrentlyStream].Height - 50;

                    W1.Top = FM[CurrentlyStream].Top + 30;
                    W1.Left = FM[CurrentlyStream].Left + 10;
                    DmF.Add(W1);
                    W1.Show();
                }
                else
                {
                    Window1 W1 = null;
                    DmF.Add(W1);
                }

            }
            catch (Exception)
            {
                if (YTB)
                {
                    string VideoTitle = getUriSteam.GetUrlTitle(roomId, "youtube");
                    NewLiveWindow(VideoTitle);
                    CurrentlyStream = int.Parse(liveIndex.Text);
                    MMPU.RoomInfo.Add(new Room.RoomInfo { Name = CurrentlyStream.ToString(), RoomNumber = roomId, steam = "youtube", status = true, Text = VideoTitle });
                    GetYoutube(roomId);

                    EditTitleVolume(CurrentlyStream, trackBar1.Value);
                    if (VLC[CurrentlyStream].GetPlayerState() == -10)
                    {
                        if (MMPU.DanmakuEnabled)
                        {
                            Window1 W1 = null;
                            DmF.Add(W1);
                        }

                        UpdateLiveForm(CurrentlyStream, roomId);
                        return;
                    }
                }
                else
                {
                    //MessageBox.Show("不能识别的房间号");
                    return;
                }
            }

        }
        public void UpdateLiveForm(int 窗口编号, string RoomId)
        {
            List<string> cs = new List<string>();
            MMPU.DMlist.Add(cs);
            FM[窗口编号].Close();
            VLC[窗口编号].Dispose();
            Thread.Sleep(1000);
            NewLive(RoomId);
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
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
                IndexRoomNum = listBox.Items[index].ToString().Split('：')[1];
                for (int i = 0; i < MMPU.RoomConfigList.Count; i++)
                {
                    if (MMPU.RoomConfigList[i].RoomNumber == IndexRoomNum.ToString())
                    {
                        NewLive(MMPU.RoomConfigList[i].RoomNumber);
                        return;
                    }
                }

            }
        }
        /// <summary>
        /// 返回选择的房间真实房间号
        /// </summary>
        /// <param name="index"></param>
        /// <param name="A">F为房间名，T为房间号</param>
        /// <returns></returns>
        private string 返回选择的房间真实房间号(int index,bool A)
        {
            A:
            try
            {
                for (int i = 0; i < MMPU.RoomConfigList.Count; i++)
                {
                    if (listBox.Items[index].ToString().Split('：')[1].Replace(MMPU.RoomConfigList[i].RoomNumber, "").Length == 0)
                    {
                       if(A)
                        {
                            return MMPU.RoomConfigList[i].RoomNumber;
                        }
                        else
                        {
                            return MMPU.RoomConfigList[i].Name;
                        }

                    }
                }
                return "-1";
            }
            catch (Exception ex)
            {
                Thread.Sleep(1000);
                goto A;
                return "-1";
            }
        }

        private int 流选择()
        {
            switch (流.Text)
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
                if (RoomNametext.Text.Length != RoomNametext.Text.Replace("：", "").Length)
                {
                    MessageBox.Show("房间名里出现了意外的字符: \"：\"");
                    return;
                }
                if (!string.IsNullOrEmpty(biliRoomId.Text))
                {
                    try
                    {
                        int.Parse(biliRoomId.Text);
                        MMPU.RoomConfigList.Add(new Room.RoomCadr() { Name = RoomNametext.Text, RoomNumber = biliRoomId.Text, status = false, Types = "bilibili", VideoStatus = false });
                    }
                    catch (Exception)
                    {
                        if (YTB)
                        {
                            MMPU.RoomConfigList.Add(new Room.RoomCadr() { Name = RoomNametext.Text, RoomNumber = biliRoomId.Text, status = false, Types = "youtube", VideoStatus = false });
                        }
                        else
                        {
                            MessageBox.Show("不能识别的房间号");
                            return;
                        }
                    }
                    SaveRoomInfo();
                    MMPU.InitializeRoomList();

                    UpdateRoomList();


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
        /// <summary>
        /// 定时刷新房间
        /// </summary>
        public void TimelyRefreshingRoomStatus()
        {
            Thread T1 = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    Thread.Sleep(60000);
                    if (!MMPU.IsRefreshed)
                    {
                        UpdateRoomList();
                    }
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
        /// <summary>
        /// 更新房间列表
        /// </summary>
        public void UpdateRoomList()
        {
            try
            {
                Thread T1 = new Thread(new ThreadStart(delegate
                {
                   
                    if (!MMPU.IsRefreshed)
                    {
                        button8.Text = "刷新中...";

                        button8.Enabled = false;
                        // listBox.Enabled = false;
                        MMPU.IsRefreshed = true;
                        //listBox.Items.Clear();
                        List<string> LSbox = new List<string>();
                        for (int i = 0; i < MMPU.RoomConfigList.Count; i++)
                        {
                            {
                                if (getUriSteam.getBiliRoomId(MMPU.RoomConfigList[i].RoomNumber, "bilibili") == "该房间未在直播")
                                {

                                    MMPU.RoomConfigList[i].status = false;
                                    LSbox.Add("[摸鱼中]○" + (MMPU.RoomConfigList[i].VideoStatus ? "★" : "") + " " + MMPU.RoomConfigList[i].Name + "：" + MMPU.RoomConfigList[i].RoomNumber);
                                    //listBox.Items.Add("[摸鱼中]○" + (MMPU.RoomConfigList[i].VideoStatus ? "★" : "") + " " + MMPU.RoomConfigList[i].Name + "：" + MMPU.RoomConfigList[i].RoomNumber);
                                }
                                else
                                {
                                    if (!MMPU.RoomConfigList[i].status)
                                    {
                                        if (!startType)
                                        {

                                            if (MMPU.RoomConfigList[i].VideoStatus)
                                            {

                                                bool KA = true;
                                                Thread T2 = new Thread(new ThreadStart(delegate {
                                                    try
                                                    {
                                                        string 真实房间号 = MMPU.RoomConfigList[i].RoomNumber;
                                                        if (MMPU.RecNum >= MMPU.RecMax)
                                                        {
                                                            BackMessage(真实房间号+"房间下载任务建立失败，因配置最多支持" + MMPU.RecMax + "线程同时下载，本次定时录播请求已取消。", "创建下载线程失败", 3000);
                                                            KA = false;
                                                            return;
                                                        }
                                                        string 标题 = getUriSteam.GetUrlTitle(真实房间号, "bilibili").Replace(@"/", "").Replace(@"\", "");
                                                        BackMessage("自动录像启动,录制文件为: " + "./tmp/" + 真实房间号 + "_" + 标题 + ".flv", MMPU.RoomConfigList[i].Name + "开始直播了", 3000);
                                                        KA = false;
                                                        while (true)
                                                        {
                                                            string Now1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                                            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                                                            string Rid = getUriSteam.GetRoomid(真实房间号);
                                                            if (Rid != "该房间未在直播")
                                                            {
                                                                MMPU.HttpDownloadFile(getUriSteam.getBiliRoomId(Rid, "bilibili"), "./tmp/" + getUriSteam.GetRoomid(真实房间号) + "_" + 标题 + "_" + time + ".flv", Rid, 标题,false);
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                        BackMessage(标题 + "录制完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生: " + 真实房间号, "结束录制", 10000);
                                                        return;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        BackMessage("一个录像录制完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生 " , "结束录制", 10000);
                                                    }

                                                }));
                                                T2.IsBackground = true;
                                                T2.Start();
                                                Thread.Sleep(5);
                                                while (KA)
                                                {
                                                    Thread.Sleep(5);
                                                }
                                            }
                                            else
                                            {
                                                BackMessage(getUriSteam.GetUrlTitle(MMPU.RoomConfigList[i].RoomNumber, "bilibili"), MMPU.RoomConfigList[i].Name + " 开始直播了", 5000);
                                            }
                                        }
                                        MMPU.RoomConfigList[i].status = !MMPU.RoomConfigList[i].status;
                                    }
                                    LSbox.Insert(0, "[直播中]●" + (MMPU.RoomConfigList[i].VideoStatus ? "★" : "") + " " + MMPU.RoomConfigList[i].Name + "：" + MMPU.RoomConfigList[i].RoomNumber);
                                    //listBox.Items.Insert(0, "[直播中]●" + (MMPU.RoomConfigList[i].VideoStatus ? "★" : "") + " " + MMPU.RoomConfigList[i].Name + "：" + MMPU.RoomConfigList[i].RoomNumber);
                                }
                            }
                        }
                        try
                        {
                            listBox.Items.Clear();
                            for (int i = 0; i < LSbox.Count; i++)
                            {
                                listBox.Items.Add(LSbox[i]);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        button8.Text = "刷新";
                        button8.Enabled = true;
                        //  listBox.Enabled = true;
                        MMPU.IsRefreshed = false;
                        startType = false;
                    }
                }));
                T1.IsBackground = true;
                T1.Start();
            }
            catch (Exception)
            {

            }

        }

        public void SaveRoomInfo()
        {
            Room.RoomBox RB = new Room.RoomBox() { data = MMPU.RoomConfigList };//房间信息
            File.WriteAllText(MMPU.RoomConfigFile, JsonConvert.SerializeObject(RB));
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
                IndexRoomNum = listBox.Items[index].ToString().Split('：')[1];
                Console.WriteLine(IndexRoomNum);
                indexRoom = index;
            }
        }

        private int getListIndex()
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].ToString().Split('：')[1] == IndexRoomNum)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 删除房间按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click_1(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("确认要删除" + listBox.Items[getListIndex()].ToString() + "？", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                listBox.Items.Remove(listBox.Items[getListIndex()]);
                for (int i = 0; i < MMPU.RoomConfigList.Count; i++)
                {
                    if (MMPU.RoomConfigList[i].RoomNumber == IndexRoomNum.ToString())
                    {
                        MMPU.RoomConfigList.RemoveAt(i);
                    }
                }


                SaveRoomInfo();
                MMPU.InitializeRoomList();
                UpdateRoomList();


            }
            else
            {
                return;
            }

        }

        /// <summary>
        /// 刷新房间按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            UpdateRoomList();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
        private string 更新并获取直播流(string RID)
        {
            string steam = getUriSteam.getBiliRoomId(RID, "bilibili");
            MMPU.RoomInfo[CurrentlyStream].steam = steam;
            return steam;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                Show();
                this.WindowState = FormWindowState.Normal; ;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                //notifyIcon1.Visible = false;
            }


            //DDTV.Visible = true;

            //WindowState = FormWindowState.Normal;

            // DDTV.Visible = false;
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
                    getUriSteam.StreamId = 0;
                    break;
                case "备线1":
                    getUriSteam.StreamId = 1;
                    break;
                case "备线2":
                    getUriSteam.StreamId = 2;
                    break;
                case "备线3":
                    getUriSteam.StreamId = 3;
                    break;
            }
        }

        private void 修改分辨率_Click(object sender, EventArgs e)
        {
            try
            {
                int X = int.Parse(窗口宽.Text);
                int Y = int.Parse(窗口高.Text);
                if (X <= 0 || X >= 1920 || Y <= 0 || Y >= 1080)
                {
                    MessageBox.Show("输入的数据过大或过小");
                    return;
                }
                for (int i = 0; i < FM.Count; i++)
                {
                    Size C = new Size(X, Y);
                    FM[i].Size = C;
                    FSize[i] = C;
                    PBOX[i].Size = new Size(C.Width - WindowTopLeft.X - 20, C.Height - WindowTopLeft.Y - 42);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("输入的内容不是有效数字");
                return;
            }

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                DDTV.Visible = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                MMPU.RoomInfo[CurrentlyStream].Top = TopInfo.Checked;
                FM[CurrentlyStream].TopMost = TopInfo.Checked;
                DmF[CurrentlyStream].Topmost = TopInfo.Checked;
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 后台系统提示气泡
        /// </summary>
        /// <param name="标题"></param>
        /// <param name="Text"></param>
        /// <param name="time"></param>
        private void BackMessage(string 标题, string Text, int time)
        {
            DDTV.Visible = true;
            DDTV.ShowBalloonTip(time, Text, 标题, ToolTipIcon.Info);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button9_Click_3(object sender, EventArgs e)
        {

        }


        private void button11_Click(object sender, EventArgs e)
        {
            if (MMPU.RecNum >= MMPU.RecMax)
            {
                BackMessage(返回选择的房间真实房间号(indexRoom, true)+"房间录制未启动，因配置最多支持" + MMPU.RecMax + "线程同时下载", "创建下载线程失败", 3000);
                return;
            }
            MessageBox.Show("录制功能仅限于自己观看学习，请勿用于商业以及其他版权方不允许的领域", "声明");
           

            Thread T1 = new Thread(new ThreadStart(delegate
            {
                try
                {
                    string 真实房间号 = 返回选择的房间真实房间号(indexRoom, true);
                    string 标题 = getUriSteam.GetUrlTitle(真实房间号, "bilibili").Replace(@"/", "").Replace(@"\", "");
                    BackMessage("录制文件为: " + "./tmp/" + 真实房间号 + "_" + 标题 + ".flv", "开始录制", 3000);
                    while (true)
                    {

                        string Now1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string Rid = getUriSteam.GetRoomid(真实房间号);
                        if (Rid != "该房间未在直播")
                        {
                            MMPU.HttpDownloadFile(getUriSteam.getBiliRoomId(Rid, "bilibili"), "./tmp/" + getUriSteam.GetRoomid(真实房间号) + "_" + 标题 + "_" + time + ".flv", Rid, 标题,false);
                        }
                        else
                        {

                            break;
                        }
                    }
                    BackMessage(标题 + "录制完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生: " + 真实房间号, "结束录制", 10000);
                    return;
                }
                catch (Exception)
                {
                    BackMessage("一个录像录制完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生 " , "结束录制", 10000);
                }


            }));
            T1.IsBackground = true;
            T1.Start();

        }

        private void button10_Click(object sender, EventArgs e)
        {


            for (int i = 0; i < MMPU.RoomConfigList.Count; i++)
            {
                if (返回选择的房间真实房间号(getListIndex(), true) == MMPU.RoomConfigList[i].RoomNumber)
                {
                    if (MMPU.RoomConfigList[i].VideoStatus)
                    {
                        MMPU.RoomConfigList[i].VideoStatus = false;
                    }
                    else
                    {
                        MMPU.RoomConfigList[i].VideoStatus = true;
                    }
                    SaveRoomInfo();
                    UpdateRoomList();
                    return;
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("AAA");
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void DMNF_CheckedChanged(object sender, EventArgs e)
        {
            MMPU.DanmakuEnabled = DMNF.Checked;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MMPU.RoomConfigList.Count; i++)
            {
                if (listBox.Items[indexRoom].ToString().Length != listBox.Items[indexRoom].ToString().Replace(MMPU.RoomConfigList[i].RoomNumber, "").Length)
                {
                    System.Diagnostics.Process.Start("https://live.bilibili.com/" + MMPU.RoomConfigList[i].RoomNumber);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            退出(e);
        }
        private void 退出(FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否确认退出程序关闭所有DD窗口？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    File.Delete("./debug.log");
                }
                catch (Exception)
                {


                }
                // 关闭所有的线程
                //this.Dispose();
                //this.Close();
                Environment.Exit(0);//结束当前进程
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (button13.Text == "开始录制")
            {
                BackMessage("正在建立YouTube下载任务....请稍候...........", "准备下载", 3000);
                YtbDloB = true;
                button13.Text = "停止录制";
                DloYTB(YTBRoomID.Text);
            }
            else
            {
                button13.Text = "开始录制";
                YtbDloB = false;
            }

        }
        public void DloYTB(string roomId)
        {
            string ASDASD = "";
            try
            {
                ASDASD = MMPU.GetUrlContent("https://www.youtube.com/channel/" + roomId + "/live");
            }
            catch (Exception)
            {
                MessageBox.Show("YouTube连接失败或该频道不存在，请确认输入的是频道，频道号应为https://www.youtube.com/channel/xxxxxxxxxxxxxxxxxxxxxxx  这种格式的channel/后面部分");
                return;
            }
            try
            {
                ASDASD = ASDASD.Replace("\\\"},\\\"playbackTracking\\\"", "㈨").Split('㈨')[0].Replace("\\\"hlsManifestUrl\\\":\\\"", "㈨").Split('㈨')[1].Replace("\",\\\"probeUrl\\\"", "㈨").Split('㈨')[0].Replace("\\", "");
            }
            catch (Exception)
            {
                //ClForm(标号);
                MessageBox.Show("该youtube频道未开启直播");
                return;
            }
            ASDASD = MMPU.GetUrlContent(ASDASD);
            /*
             * 3  256x144 
             * 5  426x240
             * 7  640x360
             * 9  854x480
             * 11 1280x720
             * 13 1920x1080
             */

            string[] ASD = ASDASD.Split('\n');
            bool 播放开始 = false;

            Thread T1 = new Thread(new ThreadStart(delegate
            {
                List<string> YDLOlist = new List<string>();
                List<string> LS1 = new List<string>();
                int S = 0;
                while (true)
                {

                    try
                    {
                        ASDASD = MMPU.GetUrlContent(ASD[CurrentResolution()]);
                    }
                    catch (Exception)
                    {
                        if (S != 0)
                        {
                            MessageBox.Show("YouTube录制已完成，该提示也有可能是因为频道推流结束引起的");
                        }
                        else
                        {
                            MessageBox.Show("该频道不存在，请确认输入的是频道，频道号应为https://www.youtube.com/channel/xxxxxxxxxxxxxxxxxxxxxxx  这种格式的channel/后面部分");
                        }

                        return;
                    }
                    /*
                     * 6   1
                     * 8   2
                     * 10  3
                     */
                    string[] LS3 = ASDASD.Split('\n');
                    List<string[]> LS2 = new List<string[]>();
                    for (int i = 0; i < LS3.Length; i++)
                    {
                        if (LS3[i].Length != LS3[i].Replace("http", "").Length)
                        {
                            string[] A = new string[2];

                            Regex reg = new Regex("index.m3u8/sq/(.+)/goap");
                            Match match = reg.Match(LS3[i]);
                            string value = match.Groups[1].Value;
                            A[0] = LS3[i];
                            A[1] = value;
                            LS2.Add(A);
                        }
                    }



                    Directory.CreateDirectory("./tmp/" + roomId + "/");
                    for (int i = 0; i < LS2.Count; i++)
                    {
                        if (!YtbDloB)
                        {
                            BackMessage("YouTube下载已停止，下载文件在软件根目录下的    /tmp/" + roomId + "/    内", "已停止下载", 5000);
                            return;
                        }
                        try
                        {
                            if (LS1.Count == 0)
                            {
                                LS1.Add(LS2[i][1]);
                                MMPU.HttpDownloadFile(LS2[i][0], "./tmp/" + roomId + "/" + LS2[i][1] + ".flv", roomId, LS2[i][1],true);
                                S++;
                                //break;
                            }
                            else if (!LS1.Contains(LS2[i][1]))
                            {
                                LS1.Add(LS2[i][1]);
                                MMPU.HttpDownloadFile(LS2[i][0], "./tmp/" + roomId + "/" + LS2[i][1] + ".flv", roomId, LS2[i][1],true);
                                S++;
                            }
                        }
                        catch (Exception)
                        {

                            BackMessage("目标流返回的数据已经接受完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生", "录制结束", 10000);
                            return;
                        }
                        if (S > 3 && !播放开始)
                        {

                            播放开始 = true;
                            Thread T2 = new Thread(new ThreadStart(delegate
                            {
                                BackMessage("YouTube开始下载，下载文件在软件根目录下的    /tmp/" + roomId + "/    内", "开始下载", 5000);
                                int S1 = 0;

                                FileStream fs = new FileStream("./tmp/" + roomId + "/" + LS1[0] + ".flv", FileMode.Append);//以Append方式打开文件
                                BinaryWriter bw = new BinaryWriter(fs); //二进制写入流
                                S1++;
                                while (true)
                                {
                                    if (!YtbDloB)
                                    {
                                        BackMessage("YouTube下载已停止，下载文件在软件根目录下的    /tmp/" + roomId + "/    内", "已停止下载", 5000);
                                        fs.Close();
                                        bw.Close();
                                        return;
                                    }
                                    try
                                    {
                                        if (File.Exists("./tmp/" + roomId + "/" + (LS1[S1 + 1]) + ".flv"))
                                        {
                                            FileStream fs1 = new FileStream("./tmp/" + roomId + "/" + LS1[S1] + ".flv", FileMode.Open);//以Append方式打开文件
                                            BinaryReader br1 = new BinaryReader(fs1);
                                            byte[] Q2 = br1.ReadBytes((int)br1.BaseStream.Length);
                                            bw.Write(Q2, 0, Q2.Length); //写文件

                                            fs1.Close();
                                            br1.Close();
                                            File.Delete("./tmp/" + roomId + "/" + LS1[S1] + ".flv");
                                            S1++;
                                        }
                                        //else
                                        //{
                                        //    if (MMPU.IsExistFile("./tmp/" + roomId + "/" + (LS1[S1] + 1) + ".flv"))
                                        //    {
                                        //        S1++;
                                        //    }

                                        //}
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }));
                            T2.IsBackground = true;
                            T2.Start();

                        }

                    }
                }

            }));
            T1.IsBackground = true;
            T1.Start();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //BackMessage("阿删掉测", "ASDSADSADSADSADAS", 0);

            //   BackMessage("123",  " 开始直播了", 5000);
            //return;
            //BackMessage("目标流返回的数据已经接受完毕，该提示一般是录制结束的提示，但是有时候会出现因为网络/直播流错误而产生", "录制结束", 3000);
            if (MessageBox.Show("制作:某米\n\n" +
                "本软件所有功能仅限于学习交流，请勿用于商业以及其他版权方不允许的领域\n\n" +
                "本软件基于GNU General Public License v3.0开源，于GitHub免费发布，点击确定的地址跳转到GitHub项目界面\n\n" +
                "注意:\n" +
                "1.如果启动后弹幕一直无法加载，可能是由于弹幕所需的cef框架加载失败，请重启本体。这个问题将在后续更新中解决。\n" +
                "2.弹幕显示超出画面边框是由于桌面分辨率不是100%引起的，这需要高DPI适配，个人精力不足....\n" +
                "3.在更新软件的时候请备份好RoomListConfig.json文件，该文件是监控房间配置文件\n" +
                "4.如果需要大量修改房间名称之类的，可以直接打开RoomListConfig.json进行修改，修改前请备份" +
                "\n" +
                "", "关于本软件", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("https://github.com/CHKZL/DDTV");
            }
            else
            {
            }
        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            //this.Visible = true;
            this.WindowState = FormWindowState.Normal; ;
            this.ShowInTaskbar = true;

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void DDTV_BalloonTipShown(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
