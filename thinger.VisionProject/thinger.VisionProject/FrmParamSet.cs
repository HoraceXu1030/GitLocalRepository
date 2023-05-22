using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using thinger.VisionDAL;
using thinger.VisionModels;
using VisionDAL;

namespace thinger.VisionProject
{
    public partial class FrmParamSet : Form
    {
        public FrmParamSet(SysParams sysParams,string path)
        {
            InitializeComponent();

            this.cmb_IPAddress.DataSource = new Zmotion().GetEhList();

            this.path = path;
            this.sysParams = sysParams;

            InitParam(this.sysParams);

        }

        private string path;

        private SysParams sysParams;

        private void InitParam(SysParams sysParams)
        {
            this.cmb_IPAddress.Text = sysParams.IPAddress;
            this.txt_CamName.Text = sysParams.CamName;
            this.txt_DrvName.Text = sysParams.DrvName;
            this.cmb_ModelName.Text = sysParams.ModelName;
            this.num_Start.Value = sysParams.StartButton;
            this.num_ZeroVel.Value = Convert.ToDecimal(sysParams.ZeroVel);
            this.num_ZeroVelMin.Value = Convert.ToDecimal(sysParams.ZeroVelMin);
            this.num_ZeroAcc.Value = Convert.ToDecimal(sysParams.ZeroAcc);
            this.num_ZeroDec.Value = Convert.ToDecimal(sysParams.ZeroDec);
            this.num_ZeroSramp.Value = Convert.ToDecimal(sysParams.ZeroSramp);

            this.num_AxisNoX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisNo);
            this.num_AxisJogVelX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisJogVel);
            this.num_AxisJogVelMinX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisJogVelMin);
            this.num_AxisJogAccX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisJogAcc);
            this.num_AxisJogDecX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisJogDec);
            this.num_AxisJogSrampX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisJogSramp);
            this.num_AxisMoveVelX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisMoveVel);
            this.num_AxisMoveVelMinX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisMoveVelMin);
            this.num_AxisMoveAccX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisMoveAcc);
            this.num_AxisMoveDecX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisMoveDec);
            this.num_AxisMoveSrampX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisMoveSramp);
            this.num_AxisMoveSrampX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.AxisMoveSramp);
            this.num_ZeroDistanceX.Value = Convert.ToDecimal(sysParams.MotionParams.XAxisParams.ZeroDistance);

            this.num_AxisNoY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisNo);
            this.num_AxisJogVelY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisJogVel);
            this.num_AxisJogVelMinY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisJogVelMin);
            this.num_AxisJogAccY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisJogAcc);
            this.num_AxisJogDecY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisJogDec);
            this.num_AxisJogSrampY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisJogSramp);
            this.num_AxisMoveVelY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisMoveVel);
            this.num_AxisMoveVelMinY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisMoveVelMin);
            this.num_AxisMoveAccY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisMoveAcc);
            this.num_AxisMoveDecY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisMoveDec);
            this.num_AxisMoveSrampY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisMoveSramp);
            this.num_AxisMoveSrampY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.AxisMoveSramp);
            this.num_ZeroDistanceY.Value = Convert.ToDecimal(sysParams.MotionParams.YAxisParams.ZeroDistance);

            this.num_AxisNoZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisNo);
            this.num_AxisJogVelZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisJogVel);
            this.num_AxisJogVelMinZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisJogVelMin);
            this.num_AxisJogAccZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisJogAcc);
            this.num_AxisJogDecZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisJogDec);
            this.num_AxisJogSrampZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisJogSramp);
            this.num_AxisMoveVelZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisMoveVel);
            this.num_AxisMoveVelMinZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisMoveVelMin);
            this.num_AxisMoveAccZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisMoveAcc);
            this.num_AxisMoveDecZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisMoveDec);
            this.num_AxisMoveSrampZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisMoveSramp);
            this.num_AxisMoveSrampZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.AxisMoveSramp);
            this.num_ZeroDistanceZ.Value = Convert.ToDecimal(sysParams.MotionParams.ZAxisParams.ZeroDistance);

            this.num_AxisNoU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisNo);
            this.num_AxisJogVelU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisJogVel);
            this.num_AxisJogVelMinU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisJogVelMin);
            this.num_AxisJogAccU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisJogAcc);
            this.num_AxisJogDecU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisJogDec);
            this.num_AxisJogSrampU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisJogSramp);
            this.num_AxisMoveVelU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisMoveVel);
            this.num_AxisMoveVelMinU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisMoveVelMin);
            this.num_AxisMoveAccU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisMoveAcc);
            this.num_AxisMoveDecU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisMoveDec);
            this.num_AxisMoveSrampU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisMoveSramp);
            this.num_AxisMoveSrampU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.AxisMoveSramp);
            this.num_ZeroDistanceU.Value = Convert.ToDecimal(sysParams.MotionParams.UAxisParams.ZeroDistance);

            this.num_Unit0.Value = Convert.ToDecimal(sysParams.Unit0);
            this.num_Unit1.Value = Convert.ToDecimal(sysParams.Unit1);
            this.num_Unit2.Value = Convert.ToDecimal(sysParams.Unit2);
            this.num_Unit3.Value = Convert.ToDecimal(sysParams.Unit3);

            this.num_AType0.Value = Convert.ToDecimal(sysParams.Atype0);
            this.num_AType1.Value = Convert.ToDecimal(sysParams.Atype1);
            this.num_AType2.Value = Convert.ToDecimal(sysParams.Atype2);
            this.num_AType3.Value = Convert.ToDecimal(sysParams.Atype3);

            this.num_InvertStep0.Value = Convert.ToDecimal(sysParams.InvertStep0);
            this.num_InvertStep1.Value = Convert.ToDecimal(sysParams.InvertStep1);
            this.num_InvertStep2.Value = Convert.ToDecimal(sysParams.InvertStep2);
            this.num_InvertStep3.Value = Convert.ToDecimal(sysParams.InvertStep3);
        }

        private void btn_Set_Click(object sender, EventArgs e)
        {
            this.sysParams.IPAddress = this.cmb_IPAddress.Text.Trim();
            this.sysParams.CamName = this.txt_CamName.Text.Trim();
            this.sysParams.DrvName = this.txt_DrvName.Text.Trim();
            this.sysParams.ModelName = this.cmb_ModelName.Text.Trim();
            this.sysParams.StartButton = Convert.ToInt32(this.num_Start.Text.Trim());
            this.sysParams.ZeroVel = Convert.ToInt32(this.num_ZeroVel.Text.Trim());
            this.sysParams.ZeroVelMin = Convert.ToInt32(this.num_ZeroVelMin.Text.Trim());
            this.sysParams.ZeroVel = Convert.ToInt32(this.num_ZeroVel.Text.Trim());
            this.sysParams.ZeroVelMin = Convert.ToInt32(this.num_ZeroVelMin.Text.Trim());
            this.sysParams.ZeroAcc = Convert.ToInt32(this.num_ZeroAcc.Text.Trim());
            this.sysParams.ZeroDec = Convert.ToInt32(this.num_ZeroDec.Text.Trim());
            this.sysParams.ZeroSramp = Convert.ToInt32(this.num_ZeroSramp.Text.Trim());

            this.sysParams.MotionParams.XAxisParams.AxisNo = Convert.ToInt16(this.num_AxisNoX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisJogVel = Convert.ToInt16(this.num_AxisJogVelX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisJogVelMin = Convert.ToInt16(this.num_AxisJogVelMinX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisJogAcc = Convert.ToInt16(this.num_AxisJogAccX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisJogDec = Convert.ToInt16(this.num_AxisJogDecX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisJogSramp = Convert.ToInt16(this.num_AxisJogSrampX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisMoveVel = Convert.ToInt16(this.num_AxisMoveVelX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisMoveVelMin = Convert.ToInt16(this.num_AxisMoveVelMinX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisMoveAcc = Convert.ToInt16(this.num_AxisMoveAccX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisMoveDec = Convert.ToInt16(this.num_AxisMoveDecX.Text.Trim());
            this.sysParams.MotionParams.XAxisParams.AxisMoveSramp = Convert.ToInt16(this.num_AxisMoveSrampX.Text.Trim());

            this.sysParams.MotionParams.YAxisParams.AxisNo = Convert.ToInt16(this.num_AxisNoY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisJogVel = Convert.ToInt16(this.num_AxisJogVelY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisJogVelMin = Convert.ToInt16(this.num_AxisJogVelMinY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisJogAcc = Convert.ToInt16(this.num_AxisJogAccY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisJogDec = Convert.ToInt16(this.num_AxisJogDecY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisJogSramp = Convert.ToInt16(this.num_AxisJogSrampY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisMoveVel = Convert.ToInt16(this.num_AxisMoveVelY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisMoveVelMin = Convert.ToInt16(this.num_AxisMoveVelMinY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisMoveAcc = Convert.ToInt16(this.num_AxisMoveAccY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisMoveDec = Convert.ToInt16(this.num_AxisMoveDecY.Text.Trim());
            this.sysParams.MotionParams.YAxisParams.AxisMoveSramp = Convert.ToInt16(this.num_AxisMoveSrampY.Text.Trim());

            this.sysParams.MotionParams.ZAxisParams.AxisNo = Convert.ToInt16(this.num_AxisNoZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisJogVel = Convert.ToInt16(this.num_AxisJogVelZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisJogVelMin = Convert.ToInt16(this.num_AxisJogVelMinZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisJogAcc = Convert.ToInt16(this.num_AxisJogAccZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisJogDec = Convert.ToInt16(this.num_AxisJogDecZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisJogSramp = Convert.ToInt16(this.num_AxisJogSrampZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisMoveVel = Convert.ToInt16(this.num_AxisMoveVelZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisMoveVelMin = Convert.ToInt16(this.num_AxisMoveVelMinZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisMoveAcc = Convert.ToInt16(this.num_AxisMoveAccZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisMoveDec = Convert.ToInt16(this.num_AxisMoveDecZ.Text.Trim());
            this.sysParams.MotionParams.ZAxisParams.AxisMoveSramp = Convert.ToInt16(this.num_AxisMoveSrampZ.Text.Trim());

            this.sysParams.MotionParams.UAxisParams.AxisNo = Convert.ToInt16(this.num_AxisNoU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisJogVel = Convert.ToInt16(this.num_AxisJogVelU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisJogVelMin = Convert.ToInt16(this.num_AxisJogVelMinU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisJogAcc = Convert.ToInt16(this.num_AxisJogAccU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisJogDec = Convert.ToInt16(this.num_AxisJogDecU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisJogSramp = Convert.ToInt16(this.num_AxisJogSrampU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisMoveVel = Convert.ToInt16(this.num_AxisMoveVelU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisMoveVelMin = Convert.ToInt16(this.num_AxisMoveVelMinU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisMoveAcc = Convert.ToInt16(this.num_AxisMoveAccU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisMoveDec = Convert.ToInt16(this.num_AxisMoveDecU.Text.Trim());
            this.sysParams.MotionParams.UAxisParams.AxisMoveSramp = Convert.ToInt16(this.num_AxisMoveSrampU.Text.Trim());

            this.sysParams.Unit0 = Convert.ToInt16(this.num_Unit0.Text.Trim());
            this.sysParams.Unit1 = Convert.ToInt16(this.num_Unit1.Text.Trim());
            this.sysParams.Unit2 = Convert.ToInt16(this.num_Unit2.Text.Trim());
            this.sysParams.Unit3 = Convert.ToInt16(this.num_Unit3.Text.Trim());
            this.sysParams.Atype0 = Convert.ToInt16(this.num_AType0.Text.Trim());
            this.sysParams.Atype1 = Convert.ToInt16(this.num_AType1.Text.Trim());
            this.sysParams.Atype2 = Convert.ToInt16(this.num_AType2.Text.Trim());
            this.sysParams.Atype3 = Convert.ToInt16(this.num_AType3.Text.Trim());
            this.sysParams.InvertStep0 = Convert.ToInt16(this.num_InvertStep0.Text.Trim());
            this.sysParams.InvertStep1 = Convert.ToInt16(this.num_InvertStep1.Text.Trim());
            this.sysParams.InvertStep2 = Convert.ToInt16(this.num_InvertStep2.Text.Trim());
            this.sysParams.InvertStep3 = Convert.ToInt16(this.num_InvertStep3.Text.Trim());

            string jsonParam = JSONHelper.EntityToJSON(this.sysParams);

            if (IniConfigHelper.WriteIniData("参数设置", "参数数据", jsonParam, path))
            {
                MessageBox.Show("参数设置成功，部分参数重启生效！", "参数设置");
            }
            else
            {
                MessageBox.Show("参数设置失败，请检查！", "参数设置");
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            InitParam(this.sysParams);
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
