using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace thinger.VisionControlLib
{
    public delegate void ZeroAxisDelegate(string axisName);

    public delegate void MoveAxisDelegate(string axisName, float distance, bool isAbsolute);
    public partial class AxisControl : UserControl
    {
        public AxisControl()
        {
            InitializeComponent();
        }

        private string axisName = "X";
        [Browsable(true)]
        [Category("自定义属性")]
        [Description("轴名称")]
        public string AxisName
        {
            get { return axisName; }
            set
            {
                axisName = value;

                this.lbl_AxisName.Text = axisName + "轴：";
            }
        }

        /// <summary>
        /// 创建清零事件
        /// </summary>
        public event ZeroAxisDelegate ZeroAxisEvent;


        public event MoveAxisDelegate MoveAxisEvent;
        private void btn_Zero_Click(object sender, EventArgs e)
        {
            ZeroAxisEvent?.Invoke(axisName);
        }

        private void btn_Move_Click(object sender, EventArgs e)
        {
            MoveAxisEvent?.Invoke(axisName, Convert.ToSingle(this.txt_Distance.Text), this.chk_Absolute.Checked);
        }
    }
}
