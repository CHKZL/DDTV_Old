using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPUCL;
using System.Threading;

namespace DD监控室
{
    public partial class RecordForm : Form
    {
        public RecordForm()
        {
            InitializeComponent();
            numericUpDown1.Value = MMPU.RecMax;
        }

        private void RecordForm_Load(object sender, EventArgs e)
        {
            this.listView1.SmallImageList = this.imageList1;  //0是完成，1是录制中
            this.listView1.Columns.Add("房间号", 90, HorizontalAlignment.Center);
            this.listView1.Columns.Add("状态", 60, HorizontalAlignment.Center);
            this.listView1.Columns.Add("文件地址", 250, HorizontalAlignment.Left);
            this.listView1.Columns.Add("开始时间", 150, HorizontalAlignment.Center);
            this.listView1.Columns.Add("结束时间", 150, HorizontalAlignment.Center);

            Thread T1 = new Thread(new ThreadStart(delegate {
            while (true)
            {
                this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                    listView1.Items.Clear();


                    for (int i = 0; i < MMPU.RecordInfo.Count; i++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = (MMPU.RecordInfo[i].Status ? 1 : 0);     //通过与imageList绑定，显示imageList中第i项图标
                        lvi.Text = MMPU.RecordInfo[i].RoomID;
                        lvi.SubItems.Add(MMPU.RecordInfo[i].Status ? "录制中" : "录制结束");
                        lvi.SubItems.Add(MMPU.RecordInfo[i].File);
                        lvi.SubItems.Add(MMPU.RecordInfo[i].StartTime);
                        lvi.SubItems.Add(MMPU.RecordInfo[i].EndTime);
                        this.listView1.Items.Add(lvi);
                    }
                    this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
                    Thread.Sleep(3000);
                }
            }));
            T1.IsBackground = true;
            T1.Start();
        }
        bool ACS = true;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(ACS)
            {
                if((int)numericUpDown1.Value>=3)
                {
                    ACS=false;
                    MessageBox.Show("过多的线程可能导致无限重复连接，如果出现无限重复，请重启软件");
                }
            }
            MMPU.RecMax = (int)numericUpDown1.Value;
        }
    }
}
