namespace DD监控室
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.T1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.流 = new System.Windows.Forms.ComboBox();
            this.TopInfo = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.修改分辨率 = new System.Windows.Forms.Button();
            this.窗口高 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.窗口宽 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.RoomNametext = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.button4 = new System.Windows.Forms.Button();
            this.biliRoomId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.liveIndex = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Resolution = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine();
            this.DDTV = new System.Windows.Forms.NotifyIcon(this.components);
            this.button9 = new System.Windows.Forms.Button();
            this.DMNF = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(10, 225);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(187, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll_1);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(597, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "更新流";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // T1
            // 
            this.T1.Enabled = false;
            this.T1.Location = new System.Drawing.Point(640, -15);
            this.T1.Name = "T1";
            this.T1.Size = new System.Drawing.Size(151, 21);
            this.T1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.TopInfo);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.RoomNametext);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.listBox);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.biliRoomId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.liveIndex);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(496, 288);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "控制";
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(453, 225);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(39, 23);
            this.button12.TabIndex = 30;
            this.button12.Text = "跳转";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(215, 253);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(149, 23);
            this.button11.TabIndex = 29;
            this.button11.Text = "录制当前选中的房间直播";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(370, 253);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(117, 23);
            this.button10.TabIndex = 27;
            this.button10.Text = "设置开播自动录像";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.流);
            this.groupBox3.Location = new System.Drawing.Point(10, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(201, 44);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bilibili";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "流选择";
            // 
            // 流
            // 
            this.流.FormattingEnabled = true;
            this.流.Items.AddRange(new object[] {
            "主线",
            "备线1",
            "备线2",
            "备线3"});
            this.流.Location = new System.Drawing.Point(67, 14);
            this.流.Name = "流";
            this.流.Size = new System.Drawing.Size(121, 20);
            this.流.TabIndex = 19;
            this.流.Text = "主线";
            this.流.SelectedIndexChanged += new System.EventHandler(this.流_SelectedIndexChanged);
            // 
            // TopInfo
            // 
            this.TopInfo.AutoSize = true;
            this.TopInfo.Location = new System.Drawing.Point(129, 203);
            this.TopInfo.Name = "TopInfo";
            this.TopInfo.Size = new System.Drawing.Size(84, 16);
            this.TopInfo.TabIndex = 25;
            this.TopInfo.Text = "锁定在最前";
            this.TopInfo.UseVisualStyleBackColor = true;
            this.TopInfo.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Enabled = false;
            this.label8.Font = new System.Drawing.Font("宋体", 9F);
            this.label8.Location = new System.Drawing.Point(434, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "by：某米";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.修改分辨率);
            this.groupBox2.Controls.Add(this.窗口高);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.窗口宽);
            this.groupBox2.Location = new System.Drawing.Point(10, 124);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(201, 42);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " 统一修改窗口大小";
            // 
            // 修改分辨率
            // 
            this.修改分辨率.Location = new System.Drawing.Point(132, 14);
            this.修改分辨率.Name = "修改分辨率";
            this.修改分辨率.Size = new System.Drawing.Size(56, 23);
            this.修改分辨率.TabIndex = 23;
            this.修改分辨率.Text = "修改";
            this.修改分辨率.UseVisualStyleBackColor = true;
            this.修改分辨率.Click += new System.EventHandler(this.修改分辨率_Click);
            // 
            // 窗口高
            // 
            this.窗口高.Location = new System.Drawing.Point(72, 15);
            this.窗口高.Name = "窗口高";
            this.窗口高.Size = new System.Drawing.Size(45, 21);
            this.窗口高.TabIndex = 24;
            this.窗口高.Text = "440";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(58, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "X";
            // 
            // 窗口宽
            // 
            this.窗口宽.Location = new System.Drawing.Point(9, 15);
            this.窗口宽.Name = "窗口宽";
            this.窗口宽.Size = new System.Drawing.Size(45, 21);
            this.窗口宽.TabIndex = 23;
            this.窗口宽.Text = "720";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(11, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "在窗口标题滚动鼠标也能修改音量";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(92, 172);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(121, 23);
            this.button8.TabIndex = 18;
            this.button8.Text = "刷新房间列表";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "房间名";
            // 
            // RoomNametext
            // 
            this.RoomNametext.Location = new System.Drawing.Point(254, 226);
            this.RoomNametext.Name = "RoomNametext";
            this.RoomNametext.Size = new System.Drawing.Size(110, 21);
            this.RoomNametext.TabIndex = 16;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(412, 225);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(39, 23);
            this.button7.TabIndex = 15;
            this.button7.Text = "删除";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(367, 225);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(43, 23);
            this.button6.TabIndex = 14;
            this.button6.Text = "添加";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // listBox
            // 
            this.listBox.Font = new System.Drawing.Font("宋体", 10F);
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(217, 20);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(270, 199);
            this.listBox.TabIndex = 13;
            this.listBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseClick);
            this.listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 45);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(198, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "获取直播流并开始监听";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // biliRoomId
            // 
            this.biliRoomId.Location = new System.Drawing.Point(55, 20);
            this.biliRoomId.Name = "biliRoomId";
            this.biliRoomId.Size = new System.Drawing.Size(156, 21);
            this.biliRoomId.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "房间号";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 172);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(65, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "截图保存";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "选择直播";
            // 
            // liveIndex
            // 
            this.liveIndex.FormattingEnabled = true;
            this.liveIndex.Location = new System.Drawing.Point(68, 201);
            this.liveIndex.Name = "liveIndex";
            this.liveIndex.Size = new System.Drawing.Size(46, 20);
            this.liveIndex.TabIndex = 4;
            this.liveIndex.SelectedIndexChanged += new System.EventHandler(this.选择直播_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.Resolution);
            this.groupBox4.Location = new System.Drawing.Point(18, 375);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(201, 44);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "youtube";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "分辨率";
            // 
            // Resolution
            // 
            this.Resolution.FormattingEnabled = true;
            this.Resolution.Items.AddRange(new object[] {
            "256x144 ",
            "426x240",
            "640x360",
            "854x480",
            "1280x720",
            "1920x1080"});
            this.Resolution.Location = new System.Drawing.Point(67, 14);
            this.Resolution.Name = "Resolution";
            this.Resolution.Size = new System.Drawing.Size(121, 20);
            this.Resolution.TabIndex = 19;
            this.Resolution.Text = "640x360";
            this.Resolution.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(737, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(52, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "关于";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(668, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "停止流";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(593, -12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "直播流";
            // 
            // skinEngine1
            // 
            this.skinEngine1.@__DrawButtonFocusRectangle = true;
            this.skinEngine1.DisabledButtonTextColor = System.Drawing.Color.Gray;
            this.skinEngine1.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText;
            this.skinEngine1.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // DDTV
            // 
            this.DDTV.Icon = ((System.Drawing.Icon)(resources.GetObject("DDTV.Icon")));
            this.DDTV.Text = "DDTV";
            this.DDTV.Visible = true;
            this.DDTV.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(581, 36);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 28;
            this.button9.Text = "button9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click_3);
            // 
            // DMNF
            // 
            this.DMNF.AutoSize = true;
            this.DMNF.Location = new System.Drawing.Point(5, 354);
            this.DMNF.Name = "DMNF";
            this.DMNF.Size = new System.Drawing.Size(234, 16);
            this.DMNF.TabIndex = 30;
            this.DMNF.Text = "打开弹幕(测试功能,对硬件有点要求..)";
            this.DMNF.UseVisualStyleBackColor = true;
            this.DMNF.CheckedChanged += new System.EventHandler(this.DMNF_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(333, 351);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(167, 12);
            this.label10.TabIndex = 31;
            this.label10.Text = "房间列表刷新完成后再操作...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 370);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.DMNF);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.T1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(520, 370);
            this.MinimumSize = new System.Drawing.Size(520, 370);
            this.Name = "Form1";
            this.Text = "DD导播中心(多路直播监控)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox T1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox liveIndex;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox biliRoomId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox RoomNametext;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.NotifyIcon DDTV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox 流;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button 修改分辨率;
        private System.Windows.Forms.TextBox 窗口高;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox 窗口宽;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox TopInfo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox Resolution;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.CheckBox DMNF;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Label label10;
    }
}

