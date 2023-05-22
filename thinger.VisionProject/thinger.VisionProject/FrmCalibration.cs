using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using thinger.VisionDAL;
using thinger.VisionModels;

namespace thinger.VisionProject
{
    /// <summary>
    /// 圆极性
    /// </summary>
    public enum CircleTransition
    {
        all,
        positive,
        negative
    }


    public enum CirclePointSelect
    {
        max,
        first,
        last
    }

    public partial class FrmCalibration : Form
    {
        public FrmCalibration(ImageHelper image, Motion motion)
        {
            InitializeComponent();

            var result = halcon.HalconInitial(this.hWindowControl1, out hWindowHandle);

            if (result.IsSuccess)
            {
                AddLog("Halcon初始化成功");
            }
            else
            {
                AddLog("Halcon初始化失败：" + result.ErrorMsg);
            }

            //初始化参数
            InitialParam();



            this.image = image;
            this.motion = motion;

            //如果相机打开OK，获取相机参数
            if (this.image.isCamOK)
            {
                this.tb_ExposureTime.Value = image.GetExposureTime();
                this.txt_ExposureTime.Text = this.tb_ExposureTime.Value.ToString();

                this.tb_Gain.Value = image.GetGain();
                this.txt_Gain.Text = this.tb_Gain.Value.ToString();

                this.chk_Trigger.Checked = image.GetTriggerMode();

            }
            else
            {
                AddLog("相机连接失败");
            }
        }

        private void InitialParam()
        {
            this.cmb_CircleTransition.DataSource = Enum.GetNames(typeof(CircleTransition));
            this.cmb_CirclePointSelect.DataSource = Enum.GetNames(typeof(CirclePointSelect));

            //初始化控件参数
            this.txt_CircleElements.Text = circleParams.circle_Elements.ToString();
            this.txt_CircleThreshold.Text = circleParams.circle_Threshold.ToString();
            this.txt_CircleSigma.Text = circleParams.circle_Sigma.ToString();
            this.cmb_CirclePointSelect.Text = circleParams.circle_Point_Select.ToString();
            this.cmb_CircleTransition.Text = circleParams.circle_Transition.ToString();

            this.txt_Angle.Text = matchParams.startAngle.ToString();
            this.txt_Range.Text = matchParams.rangeAngle.ToString();
            this.txt_Overlap.Text = matchParams.overlap.ToString();
            this.txt_Score.Text = matchParams.score.ToString();
            this.txt_NumMatchs.Text = matchParams.numMatchs.ToString();


            this.btn_XNegative.Tag = new TagEntity("X", false);
            this.btn_XPositive.Tag = new TagEntity("X", true);

            this.btn_YNegative.Tag = new TagEntity("Y", false);
            this.btn_YPositive.Tag = new TagEntity("Y", true);

            this.btn_ZNegative.Tag = new TagEntity("Z", false);
            this.btn_ZPositive.Tag = new TagEntity("Z", true);


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

        }

        /// <summary>
        /// 创建图像采集对象
        /// </summary>
        private ImageHelper image = null;

        /// <summary>
        /// 创建运动控制对象
        /// </summary>
        private Motion motion = null;

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


        //创建一个查找圆参数
        private CircleParams circleParams = new CircleParams();

        private CancellationTokenSource cts;

        private void btn_OneShot_Click(object sender, EventArgs e)
        {
            var res = image.GrabImage(ref hWindowHandle, ref hImage);

            if (res.IsSuccess == false)
            {
                AddLog("单帧采集图像失败");
            }
        }

        private void btn_OpenImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = ".bmp|*.bmp|.png|*.png|.jpg|*.jpg|.tif|*.tif|.jpeg|*.jpeg";

                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;

                    var result = image.ReadImage(path, ref hWindowHandle, ref hImage);

                    if (result.IsSuccess == false)
                    {
                        AddLog("打开图像失败：" + result.ErrorMsg);
                    }
                }
            }
        }


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

        private void btn_SaveImage_Click(object sender, EventArgs e)
        {
            var result = image.SaveImage(Application.StartupPath + "\\SaveImage", DateTime.Now.ToString("yyyyMMddHHmmssms") + ".bmp", hImage);
            if (result.IsSuccess)
            {
                AddLog("图像保存成功");
            }
            else
            {
                AddLog("图像保存失败：" + result.ErrorMsg);
            }
        }

        private void btn_SaveAs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = ".bmp|*.bmp|.png|*.png|.jpg|*.jpg|.jpeg|*.jpeg";

                saveFileDialog.DefaultExt = ".bmp";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    var result = image.SaveImage(saveFileDialog.FileName, saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf('.') + 1), hImage);
                    if (result.IsSuccess)
                    {
                        AddLog("图像保存成功");
                    }
                    else
                    {
                        AddLog("图像保存失败：" + result.ErrorMsg);
                    }
                }
            }
        }

        private void btn_CreatShapeModel_Click(object sender, EventArgs e)
        {
            this.hWindowControl1.Focus();


            //初始化模板匹配参数

            matchParams.startAngle = Convert.ToDouble(this.txt_Angle.Text.Trim());
            matchParams.rangeAngle = Convert.ToDouble(this.txt_Range.Text.Trim());

            var result = shapeModel.CreateShapeModel(hWindowHandle, hImage, matchParams);

            if (result.IsSuccess)
            {
                AddLog("创建模板成功");
            }
        }

        private void btn_FindShapeModel_Click(object sender, EventArgs e)
        {
            //判断图像
            if (!halcon.ObjectValided(hImage))
            {
                AddLog("查找模板失败：图像不存在");
                return;
            }

            if (matchParams.modelId == null)
            {
                AddLog("查找模板失败：模板不存在");
                return;
            }

            matchParams.startAngle = Convert.ToDouble(this.txt_Angle.Text.Trim());
            matchParams.rangeAngle = Convert.ToDouble(this.txt_Range.Text.Trim());
            matchParams.overlap = Convert.ToDouble(this.txt_Overlap.Text.Trim());
            matchParams.score = Convert.ToDouble(this.txt_Score.Text.Trim());
            matchParams.numMatchs = Convert.ToInt32(this.txt_NumMatchs.Text.Trim());

            this.hWindowControl1.Focus();

            var result = shapeModel.FindShapeModel(hWindowHandle, hImage, matchParams, out HTuple homMat2D);

            if (result.IsSuccess)
            {
                AddLog("查找模板成功");
            }
            else
            {
                AddLog("查找模板失败：" + result.ErrorMsg);
            }
        }

        private void btn_Grab_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                GrabImageThread();
            }, cts.Token);

            AddLog("开始连续采图");
        }

        private void GrabImageThread()
        {
            while (!cts.IsCancellationRequested)
            {
                var res = image.GrabImage(ref hWindowHandle, ref hImage);

                if (res.IsSuccess)
                {
                    //处理信息
                    //Thread.Sleep(500); 
                }
                else
                {
                    AddLog("连续采图失败");
                    break;
                }
            }
        }

        private void btn_StopGrab_Click(object sender, EventArgs e)
        {
            AddLog("停止连续采图");
            cts?.Cancel();
        }

        private void tb_ExposureTime_Scroll(object sender, EventArgs e)
        {
            this.tb_ExposureTime.Focus();

            if (image.SetExposureTime(new HTuple(this.tb_ExposureTime.Value)).IsSuccess)
            {
                this.txt_ExposureTime.Text = this.tb_ExposureTime.Value.ToString();
            }
            else
            {
                AddLog("曝光时间设置失败");
            }
        }

        private void tb_Gain_Scroll(object sender, EventArgs e)
        {
            this.tb_Gain.Focus();

            if (image.SetGain(new HTuple(this.tb_Gain.Value)).IsSuccess)
            {
                this.txt_Gain.Text = this.tb_Gain.Value.ToString();
            }
            else
            {
                AddLog("增益设置失败");
            }
        }

        private void btn_CircleROI_Click(object sender, EventArgs e)
        {
            this.hWindowControl1.Focus();

            halcon.draw_spoke3(hImage, out HObject hRegions, hWindowHandle, circleParams.circle_Elements, circleParams.circle_Caliper_Height, circleParams.circle_Caliper_Width, out circleParams.rOI_Y, out circleParams.rOI_X, out circleParams.circle_Direct);

            if (circleParams.rOI_X.Length < 4)
            {
                AddLog("创建圆ROI失败");
            }
            else
            {
                HOperatorSet.DispObj(hImage, hWindowHandle);

                //显示ROI区域
                HOperatorSet.DispObj(hRegions, hWindowHandle);

                AddLog("创建圆ROI成功");
            }

        }

        private void btn_CircleFit_Click(object sender, EventArgs e)
        {
            //准备圆参数
            circleParams.circle_Elements = Convert.ToInt32(this.txt_CircleElements.Text);
            circleParams.circle_Threshold = Convert.ToInt32(this.txt_CircleThreshold.Text);
            circleParams.circle_Sigma = Convert.ToInt32(this.txt_CircleSigma.Text);

            circleParams.circle_Transition = this.cmb_CircleTransition.Text;
            circleParams.circle_Point_Select = this.cmb_CirclePointSelect.Text;

            //拟合圆
            var result = halcon.Fit_Circle(hImage, 0, ref circleParams, out HObject result_Circle);

            if (result.IsSuccess)
            {

                HOperatorSet.DispObj(hImage, hWindowHandle);

                HOperatorSet.DispObj(result_Circle, hWindowHandle);

                AddLog("圆拟合成功，圆心坐标为：" + circleParams.circleCenter_X.D.ToString("f2") + "," + circleParams.circleCenter_Y.D.ToString("f2"));

                this.lbl_Position.Text = circleParams.circleCenter_X.D.ToString("f2") + "," + circleParams.circleCenter_Y.D.ToString("f2");
            }
            else
            {
                AddLog("圆拟合失败：" + result.ErrorMsg);
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

        private void btn_AbsoluteMove_Click(object sender, EventArgs e)
        {
            var result = motion.MoveXY(Convert.ToSingle(this.txt_XAbsolute.Text), Convert.ToSingle(this.txt_YAbsolute.Text), true);

            if (result.IsSuccess)
            {
                AddLog("执行XY轴绝对运动成功");
            }
            else
            {
                AddLog("执行XY轴绝对运动失败："+result.ErrorMsg);
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            var result = motion.StopAllAxis();

            if (result.IsSuccess)
            {
                AddLog("执行XY轴绝对停止成功");
            }
            else
            {
                AddLog("执行XY轴绝对停止失败：" + result.ErrorMsg);
            }
        }

        private void btn_Calibration_Click(object sender, EventArgs e)
        {
         
        }
    }
}
