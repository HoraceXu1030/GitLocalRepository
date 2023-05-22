using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using thinger.VisionModels;
using thinger.VisionDAL;
using thinger.VisionControlLib;
using HalconDotNet;

namespace thinger.VisionProject
{

    public enum FormNames
    {
        集中监控,
        手眼标定,
        参数设置,
        IO监控,
        系统帮助,
    }

    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer2.SplitterWidth = 1;

            updateTimer.Interval = 500;
            updateTimer.Tick += UpdateTimer_Tick;

            this.Load += FrmMain_Load;

        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (motion.initialOK)
            {
                this.lbl_XAxisPos.Text = "X轴：" + motion.GetCmdPosX() + "mm";
                this.lbl_YAxisPos.Text = "Y轴：" + motion.GetCmdPosY() + "mm";
                this.lbl_ZAxisPos.Text = "Z轴：" + motion.GetCmdPosZ() + "mm";
                this.lbl_UAxisPos.Text = "U轴：" + motion.GetCmdPosU() + "°";
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

            //--------------SysParam加载------------------
            this.sysParams = LoadSysParams();

            if (sysParams == null)
            {
                AddLog("系统参数加载失败");
                return;
            }
            AddLog("系统参数加载成功");

            motion.SetSysParams(this.sysParams);

            //--------------Motion初始化------------------

            var result = motion.InitCard();

            if (result.IsSuccess)
            {
                AddLog("板卡连接成功");
            }
            else
            {
                AddLog("板卡连接失败：" + result.ErrorMsg);
            }

            //--------------Halcon初始化------------------

            result = halcon.HalconInitial(this.hWindowControl1,out hWindowHandle);

            if (result.IsSuccess)
            {
                AddLog("Halcon初始化成功");
            }
            else
            {
                AddLog("Halcon初始化失败：" + result.ErrorMsg);
            }

            //--------------相机初始化------------------

            image = new ImageHelper(sysParams.CamName,sysParams.DrvName);

            result = image.OpenCam();

            if (result.IsSuccess)
            {
                AddLog("相机打开成功");

                result = image.GrabImage(ref hWindowHandle, ref hImage);

                if (result.IsSuccess == false)
                {
                    AddLog("单帧采集图像失败");
                }
            }
            else
            {
                AddLog("相机打开失败");
            }

            //--------------点动绑定参数------------------

            this.btn_Left.Tag = new TagEntity("X", false);
            this.btn_Right.Tag = new TagEntity("X", true);

            this.btn_behind.Tag = new TagEntity("Y", false);
            this.btn_Front.Tag = new TagEntity("Y", true);

            this.btn_Up.Tag = new TagEntity("Z", false);
            this.btn_Down.Tag = new TagEntity("Z", true);

            this.btn_RotatePos.Tag = new TagEntity("U", true);
            this.btn_RotateNeg.Tag = new TagEntity("U", false);

            foreach (var item in this.gb_JOG.Controls)
            {
                if (item is Button button)
                {
                    if (button.Tag != null)
                    {
                        button.MouseDown += btn_JOG_MouseDown;
                        button.MouseUp += btn_JOG_MouseUp;
                    }
                }
            }

            foreach (var item in this.gb_Move.Controls)
            {
                if (item is AxisControl axisControl)
                {
                    axisControl.ZeroAxisEvent += AxisControl_ZeroAxisEvent;
                    axisControl.MoveAxisEvent += AxisControl_MoveAxisEvent;     
                }
            }

            updateTimer.Start();
        }

        private void AxisControl_MoveAxisEvent(string axisName, float distance, bool isAbsolute)
        {
            var result = motion.Move(axisName,distance,isAbsolute);

            if (result.IsSuccess)
            {
                MessageBox.Show($"执行{axisName}轴运动成功", "轴运动");
            }
            else
            {
                MessageBox.Show($"执行{axisName}轴运动失败：{result.ErrorMsg}", "轴运动");
            }

            
        }

        private void AxisControl_ZeroAxisEvent(string axisName)
        {
            var result = motion.ZeroPos(axisName);

            if (result.IsSuccess)
            {
                MessageBox.Show($"执行{axisName}轴清零成功", "轴清零");
            }
            else
            {
                MessageBox.Show($"执行{axisName}轴清零失败：{result.ErrorMsg}", "轴清零");
            }
        }

        private void btn_JOG_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag != null)
                {
                    var tag = button.Tag as TagEntity;

                    if (tag != null)
                    {
                        motion.StopAxis(tag.AxisNo);
                    }
                }
            }
        }

        private void btn_JOG_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag != null)
                {
                    var tag = button.Tag as TagEntity;

                    if (tag != null)
                    {
                        motion.JogMoveAxis(tag.AxisNo, tag.Direction);
                    }
                }
            }
        }

        //创建系统参数路径
        private string path = Application.StartupPath + "\\Config\\Config.ini";

        //创建系统参数对象
        private SysParams sysParams = null;

        //创建Motion对象
        private Motion motion = Motion.GetInstance();

        //实时更新定时器
        private Timer updateTimer = new Timer();

        /// <summary>
        /// 创建图像采集对象
        /// </summary>
        private ImageHelper image = new ImageHelper();

        //创建一个HwindowHandle
        private HTuple hWindowHandle = new HTuple();

        //创建一个hImage
        private HObject hImage = new HObject();

        //创建Halcon帮助对象
        private HalconHelper halcon = new HalconHelper();

        //创建一个模板帮助对象
        private ShapeModelHelper shapeModel = new ShapeModelHelper();

        //创建模板匹配参数对象
        private MatchParams matchParams = new MatchParams();


        private SysParams LoadSysParams()
        {
            string jsonStr = IniConfigHelper.ReadIniData("参数设置", "参数数据", JSONHelper.EntityToJSON(new SysParams()), path);

            if (jsonStr.Length > 0)
            {
                return JSONHelper.JSONToEntity<SysParams>(jsonStr);
            }
            else
            {
                return null;
            }
        }

        private void OpenWindow(FormNames formNames)
        {
            //判断是否找到
            bool isFind = false;

            //获取MainPanel里窗体的数量
            int total = this.MainPanel.Controls.Count;

            //已经关闭
            int closeCount = 0;

            for (int i = 0; i < total; i++)
            {
                Control ct = this.MainPanel.Controls[i - closeCount];

                if (ct is Form frm)
                {
                    //如果是我们需要的，应该怎么处理
                    if (frm.Text == formNames.ToString())
                    {
                        frm.BringToFront();
                        isFind = true;
                    }
                    // 其他的，直接关闭
                    else
                    {
                        frm.Close();
                        closeCount++;
                    }
                }
            }

            if (isFind == false)
            {
                Form frm = null;

                switch (formNames)
                {
                    case FormNames.集中监控:
                        break;
                    case FormNames.手眼标定:
                        frm = new FrmCalibration(this.image,this.motion );
                        break;
                    case FormNames.参数设置:
                        frm = new FrmParamSet(sysParams, path);
                        break;
                    case FormNames.IO监控:
                        // frm = new FrmIOMonitor();
                        break;

                    case FormNames.系统帮助:
                        //  frm = new FrmHelper();
                        break;
                    default:
                        break;
                }

                if (frm != null)
                {
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    frm.Parent = this.MainPanel;
                    frm.BringToFront();
                    frm.Show();
                }
            }
        }

        private void btn_Monitor_Click(object sender, EventArgs e)
        {
            OpenWindow(FormNames.集中监控);
        }

        private void btn_Calibration_Click(object sender, EventArgs e)
        {
            OpenWindow(FormNames.手眼标定);
        }

        private void btn_ParamSet_Click(object sender, EventArgs e)
        {
            OpenWindow(FormNames.参数设置);
        }



        #region 添加日志
        //系统当前时间
        private string CurrentTime
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="info"></param>
        private void AddLog(string info)
        {
            if (this.lst_Info.InvokeRequired)
            {
                this.lst_Info.Invoke(new Action(() =>
                {
                    ListViewItem listViewItem = new ListViewItem((this.lst_Info.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(CurrentTime);
                    listViewItem.SubItems.Add(info);
                    this.lst_Info.Items.Add(listViewItem);
                    this.lst_Info.Items[this.lst_Info.Items.Count - 1].EnsureVisible();
                }));
            }
            else
            {
                ListViewItem listViewItem = new ListViewItem((this.lst_Info.Items.Count + 1).ToString());
                listViewItem.SubItems.Add(CurrentTime);
                listViewItem.SubItems.Add(info);
                this.lst_Info.Items.Add(listViewItem);
                this.lst_Info.Items[this.lst_Info.Items.Count - 1].EnsureVisible();
            }

        }
        #endregion

        #region 无边框拖动

        Point mPoint;

        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        #endregion

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class TagEntity
    {
        public TagEntity(string AxisNo, bool Direction)
        {
            this.AxisNo = AxisNo;
            this.Direction = Direction;
        }

        public string AxisNo { get; set; }

        public bool Direction { get; set; }
    }


}
