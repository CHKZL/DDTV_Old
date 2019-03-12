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
                    else if (现在的时间 > 43200 && 现在的时间 < 45000)
                    {
                        this.Text = "DD三十分";
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

            读取房间配置信息并加载到内存();
            直播流的数量++;
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
        }

        private void A_Activated(object sender, EventArgs e)
        {
            MessageBox.Show("新窗口的Load事件下的信息框");//加载新窗口时显示信息框
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
            if (选择直播.Text == "添加一个新监控")
            {

                新建一个直播监听("");
            }
            else
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


        }

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
                A.Size = new Size(720, 480);
                A.Text = "DDTV-" + (当前选择的直播流).ToString() + " 当前直播的节目是：" + 标题;
                A.Icon = new Icon("./DDTV.ico");
                A.Show();
                A.Name = (当前选择的直播流).ToString();
                FSize.Add(new Size(720, 480));
                FM.Add(A);



                PictureBox p = new PictureBox();//实例化一个照片框
                p.BackColor = Color.Black;
                FM[当前选择的直播流].Controls.Add(p);//添加到当前窗口集合中
                p.Size = new Size(A.Width - 界面左上角坐标.X - 20, A.Height - 界面左上角坐标.Y - 42);
                p.Location = 界面左上角坐标;
                PBOX.Add(p);


                VLCPlayer vlcPlayer = new VLCPlayer();
                VLC.Add(vlcPlayer);

                A.ResizeEnd += A_ResizeEnd;
                A.Activated += A_Activated1;
            }
        }

        private void A_ResizeEnd(object sender, EventArgs e)
        {
            Form F1 = sender as Form;
            int 标号 = int.Parse(F1.Name);

            try
            {
                if (F1.Visible && F1.Name != "0")
                {
                    if (F1.Size != FSize[int.Parse(F1.Name)])
                    {

                        当前选择的直播流 = 标号;
                        更新控件大小();
                        FSize[int.Parse(F1.Name)] = F1.Size;
                    }

                    if (VLC[标号].获取播放状态() == -10)
                    {
                        VLC[标号].Stop();
                        选择直播.Items.Remove(标号.ToString());
                        RInfo[标号].Ty = false;
                        更新直播窗体(标号, 最近一个操作的房间号);
                        // RInfo[标号].Ty = false;
                        return;
                    }

                }
                else if (!F1.Visible && F1.Name != "0")
                {
                    VLC[标号].Stop();
                    选择直播.Items.Remove(标号.ToString());
                    RInfo[标号].Ty = false;
                    return;
                }

                Thread.Sleep(5);
            }
            catch (Exception)
            {
                try
                {
                    RInfo[标号].Ty = false;
                    VLC[标号].Stop();
                    选择直播.Items.Remove(标号.ToString());
                }
                catch (Exception)
                {
                }
                return;
            }

        }

        private void A_Activated1(object sender, EventArgs e)
        {
            Form A = sender as Form;
            当前选择的直播流 = int.Parse(A.Name);
            选择直播.Text = (当前选择的直播流).ToString();
        }




        //object sender, EventArgs e
        private EventHandler A_ResizeEndAA(Form F1)
        {
            Thread T1 = new Thread(new ThreadStart(delegate
              {
                  int 标号 = int.Parse(F1.Name);
                  while (true)
                  {
                      try
                      {
                          if (F1.Visible && F1.Name != "0")
                          {

                              if (F1.Size != FSize[int.Parse(F1.Name)])
                              {

                                  当前选择的直播流 = 标号;
                                  更新控件大小();
                                  FSize[int.Parse(F1.Name)] = F1.Size;
                              }

                              if (VLC[标号].获取播放状态() == -10)
                              {
                                  VLC[标号].Stop();
                                  选择直播.Items.Remove(标号.ToString());
                                  RInfo[标号].Ty = false;
                                  更新直播窗体(标号, 最近一个操作的房间号);
                                  // RInfo[标号].Ty = false;
                                  return;
                              }

                          }
                          else if (!F1.Visible && F1.Name != "0")
                          {
                              VLC[标号].Stop();
                              选择直播.Items.Remove(标号.ToString());
                              RInfo[标号].Ty = false;
                              return;
                          }

                          Thread.Sleep(5);
                      }
                      catch (Exception)
                      {
                          try
                          {
                              RInfo[标号].Ty = false;
                              VLC[标号].Stop();
                              选择直播.Items.Remove(标号.ToString());
                          }
                          catch (Exception)
                          {
                          }
                          return;
                      }
                  }
              }));
            T1.IsBackground = true;
            T1.Start();
            return null;
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
        }
        public void 更新直播窗体(int 窗口编号, string RoomId)
        {
            FM[窗口编号].Close();
            DialogResult result = MessageBox.Show("为房间" + RoomId + "创建多媒体窗口时解码器返回错误，这个问题是由解码器产生的\n\n窗口创建需要用户手动操作，再次开始监听房间", "解码器内部错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);


            //Thread.Sleep(10);
            //打开新的直播窗体(RoomId);

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
                            DDTV.ShowBalloonTip(3000, Roomlist[i].Name + " 开始直播了", 标题, ToolTipIcon.Info);

                            Roomlist[i].Ty = !Roomlist[i].Ty;
                        }
                        listBox.Items.Add("[直播中]● " + Roomlist[i].Name + "：" + Roomlist[i].RoomNumber);
                    }
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
        private void 储存房间信息到硬盘()
        {
            RB.data = Roomlist;
            FileStream fs = new FileStream("./RoomListConfig.ini", FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(RB));
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }

        private void 读取房间配置信息并加载到内存()
        {
            string str;
            StreamReader sr = new StreamReader(Application.StartupPath + "./RoomListConfig.ini", true);
            str = sr.ReadLine().ToString();
            sr.Close();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
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
        public int 选中的房间目录 = 0;
        private void listBox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                选中的房间目录 = index;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            listBox.Items.Remove(listBox.Items[选中的房间目录]);
            Roomlist.RemoveAt(选中的房间目录);
            更新房间列表();
            储存房间信息到硬盘();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            更新房间列表();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //BalloonTip

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
    }
}
