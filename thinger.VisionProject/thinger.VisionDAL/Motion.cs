using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.VisionModels;
using VisionDAL;

namespace thinger.VisionDAL
{
    public class Motion
    {
        #region 单例模式

        //创建一个静态对象保存当前的实例
        private static Motion instance;

        //定义一个锁
        private static readonly object objlock = new object();

        //创建一个私有的构造方法
        private Motion()
        {
        }

        public static Motion GetInstance()
        {
            if (instance == null)
            {
                lock (objlock)
                {
                    if (instance == null)
                    {
                        instance = new Motion();
                    }
                }
            }
            return instance;
        }

        #endregion

        #region 字段属性

        //创建一个Motion对象
        private Zmotion motion = new Zmotion();

        /// <summary>
        /// 运动相关参数
        /// </summary>
        public MotionParams param
        {
            get { return sysParams.MotionParams; }
        }

        /// <summary>
        /// 系统所有参数
        /// </summary>
        public SysParams sysParams = null;

        //初始化状态
        public bool initialOK
        {
            get { return motion.InitedOK; }
        }


        /// <summary>
        /// 设置系统参数
        /// </summary>
        /// <param name="sysParams"></param>
        public void SetSysParams(SysParams sysParams)
        {
            this.sysParams = sysParams;

            motion.IPAddress = sysParams.IPAddress;

            motion.Unit0 = sysParams.Unit0;
            motion.Unit1 = sysParams.Unit1;
            motion.Unit2 = sysParams.Unit2;
            motion.Unit3 = sysParams.Unit3;

            motion.Atype0 = sysParams.Atype0;
            motion.Atype1 = sysParams.Atype1;
            motion.Atype2 = sysParams.Atype2;
            motion.Atype3 = sysParams.Atype3;


            motion.InvertStep0 = sysParams.InvertStep0;
            motion.InvertStep1 = sysParams.InvertStep1;
            motion.InvertStep2 = sysParams.InvertStep2;
            motion.InvertStep3 = sysParams.InvertStep3;

        }


        #endregion

        #region 板卡控制

        /// <summary>
        /// 初始化卡
        /// </summary>
        /// <returns></returns>
        public OperationResult InitCard()
        {
            return motion.InitCard();
        }

        /// <summary>
        /// 关闭卡
        /// </summary>
        /// <returns></returns>
        public OperationResult CloseCard()
        {
            return motion.CloseCard();
        }
        #endregion

        #region 点动控制

        /// <summary>
        /// X轴点动
        /// </summary>
        /// <param name="dir">正向为True，反向为False</param>
        /// <returns>操作结果</returns>
        public OperationResult JogMoveAxisX(bool dir)
        {
            return motion.VMove(param.XAxisParams.AxisNo, param.XAxisParams.AxisJogVel, dir, param.XAxisParams.AxisJogVel, param.XAxisParams.AxisJogAcc, param.XAxisParams.AxisJogDec, param.XAxisParams.AxisJogSramp);
        }

        /// <summary>
        /// Y轴点动
        /// </summary>
        /// <param name="dir">正向为True，反向为False</param>
        /// <returns>操作结果</returns>
        public OperationResult JogMoveAxisY(bool dir)
        {
            return motion.VMove(param.YAxisParams.AxisNo, param.YAxisParams.AxisJogVel, dir, param.YAxisParams.AxisJogVel, param.YAxisParams.AxisJogAcc, param.YAxisParams.AxisJogDec, param.YAxisParams.AxisJogSramp);
        }

        /// <summary>
        /// Z轴点动
        /// </summary>
        /// <param name="dir">正向为True，反向为False</param>
        /// <returns>操作结果</returns>
        public OperationResult JogMoveAxisZ(bool dir)
        {
            return motion.VMove(param.ZAxisParams.AxisNo, param.ZAxisParams.AxisJogVel, dir, param.ZAxisParams.AxisJogVel, param.ZAxisParams.AxisJogAcc, param.ZAxisParams.AxisJogDec, param.ZAxisParams.AxisJogSramp);
        }

        /// <summary>
        /// U轴点动
        /// </summary>
        /// <param name="dir">正向为True，反向为False</param>
        /// <returns>操作结果</returns>
        public OperationResult JogMoveAxisU(bool dir)
        {
            var result = motion.VMove(param.ZAxisParams.AxisNo, param.UAxisParams.AxisJogVel, !dir, param.UAxisParams.AxisJogVel, param.UAxisParams.AxisJogAcc, param.UAxisParams.AxisJogDec, param.UAxisParams.AxisJogSramp); ;

            if (result.IsSuccess)
            {
                return motion.VMove(param.UAxisParams.AxisNo, param.UAxisParams.AxisJogVel, dir, param.UAxisParams.AxisJogVel, param.UAxisParams.AxisJogAcc, param.UAxisParams.AxisJogDec, param.UAxisParams.AxisJogSramp);
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// XYZ轴点动
        /// </summary>
        /// <param name="name">轴名称</param>
        /// <param name="dir">方向</param>
        /// <returns>操作结果</returns>
        public OperationResult JogMoveAxis(string name, bool dir)
        {
            name = name.ToUpper();
            if (name.StartsWith("X"))
            {
                return JogMoveAxisX(dir);
            }
            else if (name.StartsWith("Y"))
            {
                return JogMoveAxisY(dir);
            }
            else if (name.StartsWith("Z"))
            {
                return JogMoveAxisZ(dir);
            }
            else if (name.StartsWith("U"))
            {
                return JogMoveAxisU(dir);
            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "参数名称不正确，没有以XYZU开头"
            };
        }


        /// <summary>
        /// 停止轴
        /// </summary>
        /// <param name="name">轴名称</param>
        /// <returns></returns>
        public OperationResult StopAxis(string name)
        {
            name = name.ToUpper();
            if (name.StartsWith("X"))
            {
                return motion.StopAxis(param.XAxisParams.AxisNo);
            }
            else if (name.StartsWith("Y"))
            {
                return motion.StopAxis(param.YAxisParams.AxisNo);
            }
            if (name.StartsWith("Z"))
            {
                return motion.StopAxis(param.ZAxisParams.AxisNo);
            }
            if (name.StartsWith("U"))
            {
                var result = motion.StopAxis(param.ZAxisParams.AxisNo);
                if (result.IsSuccess)
                {
                    return motion.StopAxis(param.UAxisParams.AxisNo);
                }
                else
                {
                    return result;
                }
            }
            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "参数名称不正确，没有以XYZU开头"
            };
        }

        /// <summary>
        /// 停止所有轴
        /// </summary>
        /// <param name="name">轴名称</param>
        /// <returns></returns>
        public OperationResult StopAllAxis()
        {
            return motion.StopAllAxis();
        }


            #endregion

            #region 位置清零
            public OperationResult ZeroPos(short axis)
        {
            return motion.ZeroPos(axis);
        }

        public OperationResult ZeroPos(string name)
        {
            name = name.ToUpper();
            if (name.StartsWith("X"))
            {
                return ZeroPosX();
            }
            else if (name.StartsWith("Y"))
            {
                return ZeroPosY();
            }
            if (name.StartsWith("Z"))
            {
                var res = ZeroPosZ();
                if (res.IsSuccess)
                {
                    return ZeroPosU();
                }
                return res;
            }
            if (name.StartsWith("U"))
            {
                return ZeroPosU();
            }
            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "参数名称不正确，没有以XYZU开头"
            };
        }

        public OperationResult ZeroPosX()
        {
            return motion.ZeroPos(param.XAxisParams.AxisNo);
        }

        public OperationResult ZeroPosY()
        {
            return motion.ZeroPos(param.YAxisParams.AxisNo);
        }

        public OperationResult ZeroPosZ()
        {
            return motion.ZeroPos(param.ZAxisParams.AxisNo);
        }

        public OperationResult ZeroPosU()
        {
            return motion.ZeroPos(param.UAxisParams.AxisNo);
        }
        #endregion

        #region 运动控制
        public OperationResult Move(string name, float distance, bool mode)
        {
            name = name.ToUpper();
            if (name.StartsWith("X"))
            {
                if (mode)
                {
                    return motion.MoveAbs(param.XAxisParams.AxisNo, param.XAxisParams.AxisMoveVel, distance, param.XAxisParams.AxisMoveVelMin, param.XAxisParams.AxisMoveAcc, param.XAxisParams.AxisMoveDec, param.XAxisParams.AxisMoveSramp);
                }
                else
                {
                    return motion.MoveRelative(param.XAxisParams.AxisNo, param.XAxisParams.AxisMoveVel, distance, param.XAxisParams.AxisMoveVelMin, param.XAxisParams.AxisMoveAcc, param.XAxisParams.AxisMoveDec, param.XAxisParams.AxisMoveSramp);
                }
            }
            else if (name.StartsWith("Y"))
            {
                if (mode)
                {
                    return motion.MoveAbs(param.YAxisParams.AxisNo, param.YAxisParams.AxisMoveVel, distance, param.YAxisParams.AxisMoveVelMin, param.YAxisParams.AxisMoveAcc, param.YAxisParams.AxisMoveDec, param.YAxisParams.AxisMoveSramp);
                }
                else
                {
                    return motion.MoveRelative(param.YAxisParams.AxisNo, param.YAxisParams.AxisMoveVel, distance, param.YAxisParams.AxisMoveVelMin, param.YAxisParams.AxisMoveAcc, param.YAxisParams.AxisMoveDec, param.YAxisParams.AxisMoveSramp);
                }
            }
            if (name.StartsWith("Z"))
            {
                if (mode)
                {
                    return motion.MoveAbs(param.ZAxisParams.AxisNo, param.ZAxisParams.AxisMoveVel, distance, param.ZAxisParams.AxisMoveVelMin, param.ZAxisParams.AxisMoveAcc, param.ZAxisParams.AxisMoveDec, param.ZAxisParams.AxisMoveSramp);
                }
                else
                {
                    return motion.MoveRelative(param.ZAxisParams.AxisNo, param.ZAxisParams.AxisMoveVel, distance, param.ZAxisParams.AxisMoveVelMin, param.ZAxisParams.AxisMoveAcc, param.ZAxisParams.AxisMoveDec, param.ZAxisParams.AxisMoveSramp);
                }
            }
            if (name.StartsWith("U"))
            {
                if (mode)
                {
                    return motion.MoveAbs(param.UAxisParams.AxisNo, param.UAxisParams.AxisMoveVel, distance, param.UAxisParams.AxisMoveVelMin, param.UAxisParams.AxisMoveAcc, param.UAxisParams.AxisMoveDec, param.UAxisParams.AxisMoveSramp);
                }
                else
                {
                    return motion.MoveRelative(param.UAxisParams.AxisNo, param.UAxisParams.AxisMoveVel, distance, param.UAxisParams.AxisMoveVelMin, param.UAxisParams.AxisMoveAcc, param.UAxisParams.AxisMoveDec, param.UAxisParams.AxisMoveSramp);
                }
            }
            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "参数名称不正确，没有以XYZU开头"
            };

        }

        #endregion

        #region 获取轴的实时位置

        public string GetCmdPosX()
        {
            return motion.GetPos(param.XAxisParams.AxisNo).ToString("f1");
        }

        public string GetCmdPosY()
        {
            return motion.GetPos(param.YAxisParams.AxisNo).ToString("f1");
        }

        public string GetCmdPosZ()
        {
            return (motion.GetPos(param.ZAxisParams.AxisNo) + motion.GetPos(param.UAxisParams.AxisNo)).ToString("f1");
        }

        public string GetCmdPosU()
        {
            return motion.GetPos(param.UAxisParams.AxisNo).ToString("f1");
        }

        #endregion

        #region XY定位
        public OperationResult MoveXY(float xPos, float yPos, bool mode)
        {
            if (mode)
            {
                return motion.MoveLineAbs(new int[] { param.XAxisParams.AxisNo, param.YAxisParams.AxisNo }, new float[] { xPos, yPos }, param.XAxisParams.AxisMoveVel, param.XAxisParams.AxisMoveAcc, param.XAxisParams.AxisMoveDec);
            }
            else
            {
                return motion.MoveLineRelative(new int[] { param.XAxisParams.AxisNo, param.YAxisParams.AxisNo }, new float[] { xPos, yPos }, param.XAxisParams.AxisMoveVel, param.XAxisParams.AxisMoveAcc, param.XAxisParams.AxisMoveDec);
            }
        }
        
        }
        #endregion

    }
}
