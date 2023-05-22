using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using thinger.VisionModels;

namespace VisionDAL
{
    public class Zmotion
    {
        public Zmotion()
        {

        }

        private IntPtr Handle;

        public string IPAddress { get; set; } = "192.168.0.11";

        public bool InitedOK { get; set; } = false;

        //脉冲当量
        public int Unit0 { get; set; } = 400;
        public int Unit1 { get; set; } = 400;
        public int Unit2 { get; set; } = 400;
        public int Unit3 { get; set; } = 400;


        //轴类型
        public int Atype0 { get; set; } = 1;
        public int Atype1 { get; set; } = 1;
        public int Atype2 { get; set; } = 1;
        public int Atype3 { get; set; } = 1;


        //脉冲类型
        public int InvertStep0 { get; set; } = 5;
        public int InvertStep1 { get; set; } = 5;
        public int InvertStep2 { get; set; } = 1;
        public int InvertStep3 { get; set; } = 1;




        /// <summary>
        /// 搜索IP地址集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetEhList()
        {
            StringBuilder ipAddressList = new StringBuilder(100);

            uint addbufferlength = 1000;

            int error = zmcaux.ZAux_SearchEthlist(ipAddressList, addbufferlength, 10);

            if (error == 0)
            {
                string result = ipAddressList.ToString().Trim();

                if (result.Contains(' '))
                {

                    return result.Split(' ').ToList();
                }
                else
                {
                    return new List<string>() { result };
                }

            }
            return new List<string>();
        }

        /// <summary>
        /// 初始化板卡
        /// </summary>
        /// <returns></returns>
        public OperationResult InitCard()
        {
            if (!Ping(this.IPAddress))
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = "IP地址Ping失败"
                };
            }

            int error = zmcaux.ZAux_OpenEth(this.IPAddress, out Handle);
            InitedOK = true;
            if (error == 0 && Handle != IntPtr.Zero)
            {
                InitedOK = true;

                error = zmcaux.ZAux_Direct_SetUnits(Handle, 0, Unit0);
                ErrorHandler("ZAux_Direct_SetUnits", error);

                error = zmcaux.ZAux_Direct_SetUnits(Handle, 1, Unit1);
                ErrorHandler("ZAux_Direct_SetUnits", error);

                error = zmcaux.ZAux_Direct_SetUnits(Handle, 2, Unit2);
                ErrorHandler("ZAux_Direct_SetUnits", error);

                error = zmcaux.ZAux_Direct_SetUnits(Handle, 3, Unit3);
                ErrorHandler("ZAux_Direct_SetUnits", error);


                error = zmcaux.ZAux_Direct_SetAtype(Handle, 0, Atype0);
                ErrorHandler("ZAux_Direct_SetAtype", error);

                error = zmcaux.ZAux_Direct_SetAtype(Handle, 1, Atype1);
                ErrorHandler("ZAux_Direct_SetAtype", error);

                error = zmcaux.ZAux_Direct_SetAtype(Handle, 2, Atype2);
                ErrorHandler("ZAux_Direct_SetAtype", error);

                error = zmcaux.ZAux_Direct_SetAtype(Handle, 3, Atype3);
                ErrorHandler("ZAux_Direct_SetAtype", error);


                error = zmcaux.ZAux_Direct_SetInvertStep(Handle, 0, InvertStep0);
                ErrorHandler("ZAux_Direct_SetInvertStep", error);

                error = zmcaux.ZAux_Direct_SetInvertStep(Handle, 1, InvertStep1);
                ErrorHandler("ZAux_Direct_SetInvertStep", error);

                error = zmcaux.ZAux_Direct_SetInvertStep(Handle, 2, InvertStep2);
                ErrorHandler("ZAux_Direct_SetInvertStep", error);

                error = zmcaux.ZAux_Direct_SetInvertStep(Handle, 3, InvertStep3);
                ErrorHandler("ZAux_Direct_SetInvertStep", error);

                error = zmcaux.ZAux_Direct_SetInvertIn(Handle, 28, 1);
                ErrorHandler("ZAux_Direct_SetInvertIn", error);

                error = zmcaux.ZAux_Direct_SetInvertIn(Handle, 29, 1);
                ErrorHandler("ZAux_Direct_SetInvertIn", error);

                error = zmcaux.ZAux_Direct_SetInvertIn(Handle, 30, 1);
                ErrorHandler("ZAux_Direct_SetInvertIn", error);

                error = zmcaux.ZAux_Direct_SetInvertIn(Handle, 31, 1);
                ErrorHandler("ZAux_Direct_SetInvertIn", error);

                return OperationResult.CreateSuccessResult();
            }
            else
            {
                InitedOK = false;
                return OperationResult.CreateFailResult();
            }
        }

        private bool Ping(string host)
        {
            try
            {
                //------------使用ping类------
                Ping p1 = new Ping();
                PingReply reply = p1.Send(host); //发送主机名或Ip地址
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 关闭板卡
        /// </summary>
        /// <returns></returns>
        public OperationResult CloseCard()
        {

            if (zmcaux.ZAux_Close(Handle) == 0)
            {
                Handle = IntPtr.Zero;
                InitedOK = false;
                return OperationResult.CreateSuccessResult();
            }
            return OperationResult.CreateFailResult();
        }


        /// <summary>
        /// 连续运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="vel">运行速度</param>
        /// <param name="dir">方向</param>
        /// <param name="velMin">最小速度</param>
        /// <param name="acc">加速度</param>
        /// <param name="dec">减速度</param>
        /// <param name="sramp">S曲线时间</param>
        /// <returns>操作结果</returns>
        public OperationResult VMove(short axis, float vel, bool dir, float velMin, float acc, float dec, float sramp)
        {
            // 判断是否满足运动条件
            var result = CommonMotionValidate(axis);

            if (!result.IsSuccess) return result;

            //创建错误码
            int error = 0;

            try
            {
                //设置最小速度
                error = zmcaux.ZAux_Direct_SetLspeed(Handle, axis, velMin);
                ErrorHandler("ZAux_Direct_SetLspeed", error);

                //设置运行速度
                error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis, vel);
                ErrorHandler("ZAux_Direct_SetSpeed", error);

                //设置加速度
                error = zmcaux.ZAux_Direct_SetAccel(Handle, axis, acc);
                ErrorHandler("ZAux_Direct_SetAccel", error);

                //设置减速度
                error = zmcaux.ZAux_Direct_SetDecel(Handle, axis, dec);
                ErrorHandler("ZAux_Direct_SetDecel", error);

                //设置S曲线
                error = zmcaux.ZAux_Direct_SetSramp(Handle, axis, sramp);
                ErrorHandler("ZAux_Direct_SetSramp", error);

                //设置方向并运动
                error = zmcaux.ZAux_Direct_Single_Vmove(Handle, axis, dir ? 1 : -1);
                ErrorHandler("ZAux_Direct_Single_Vmove", error);

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }


        /// <summary>
        /// 相对运动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vel"></param>
        /// <param name="distance"></param>
        /// <param name="velMin"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="sramp"></param>
        /// <returns></returns>
        public OperationResult MoveRelative(short axis, float vel, float distance, float velMin, float acc, float dec, float sramp)
        {
            // 判断是否满足运动条件
            var result = CommonMotionValidate(axis);

            if (!result.IsSuccess) return result;

            //创建错误码
            int error = 0;

            try
            {
                //设置最小速度
                error = zmcaux.ZAux_Direct_SetLspeed(Handle, axis, velMin);
                ErrorHandler("ZAux_Direct_SetLspeed", error);

                //设置运行速度
                error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis, vel);
                ErrorHandler("ZAux_Direct_SetSpeed", error);

                //设置加速度
                error = zmcaux.ZAux_Direct_SetAccel(Handle, axis, acc);
                ErrorHandler("ZAux_Direct_SetAccel", error);

                //设置减速度
                error = zmcaux.ZAux_Direct_SetDecel(Handle, axis, dec);
                ErrorHandler("ZAux_Direct_SetDecel", error);

                //设置S曲线
                error = zmcaux.ZAux_Direct_SetSramp(Handle, axis, sramp);
                ErrorHandler("ZAux_Direct_SetSramp", error);

                //设置方向并运动
                error = zmcaux.ZAux_Direct_Single_Move(Handle, axis, distance);
                ErrorHandler("ZAux_Direct_Single_Move", error);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }


        /// <summary>
        /// 绝对运动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vel"></param>
        /// <param name="pos"></param>
        /// <param name="velMin"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="sramp"></param>
        /// <returns></returns>
        public OperationResult MoveAbs(short axis, float vel, float pos, float velMin, float acc, float dec, float sramp)
        {
            // 判断是否满足运动条件
            var result = CommonMotionValidate(axis);

            if (!result.IsSuccess) return result;

            //创建错误码
            int error = 0;

            try
            {
                //设置最小速度
                error = zmcaux.ZAux_Direct_SetLspeed(Handle, axis, velMin);
                ErrorHandler("ZAux_Direct_SetLspeed", error);

                //设置运行速度
                error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis, vel);
                ErrorHandler("ZAux_Direct_SetSpeed", error);

                //设置加速度
                error = zmcaux.ZAux_Direct_SetAccel(Handle, axis, acc);
                ErrorHandler("ZAux_Direct_SetAccel", error);

                //设置减速度
                error = zmcaux.ZAux_Direct_SetDecel(Handle, axis, dec);
                ErrorHandler("ZAux_Direct_SetDecel", error);

                //设置S曲线
                error = zmcaux.ZAux_Direct_SetSramp(Handle, axis, sramp);
                ErrorHandler("ZAux_Direct_SetSramp", error);

                //设置方向并运动
                error = zmcaux.ZAux_Direct_Single_MoveAbs(Handle, axis, pos);
                ErrorHandler("ZAux_Direct_Single_MoveAbs", error);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }

        /// <summary>
        /// 2轴相对定位
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vel"></param>
        /// <param name="distance"></param>
        /// <param name="velMin"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="sramp"></param>
        /// <returns></returns>
        public OperationResult Move2DRelative(short[] axis, float[] vel, float[] distance, float[] velMin, float[] acc, float[] dec, float[] sramp)
        {
            if (axis.Length == 2 && vel.Length == 2 && distance.Length == 2 && velMin.Length == 2 && acc.Length == 2 && dec.Length == 2 && sramp.Length == 2)
            {
                OperationResult result = new OperationResult();

                //相对定位
                for (int i = 0; i < 2; i++)
                {
                    result = MoveRelative(axis[i], vel[i], distance[i], velMin[i], acc[i], dec[i], sramp[i]);

                    if (!result.IsSuccess) return result;
                }

                //等待停止
                for (int i = 0; i < 2; i++)
                {
                    result = WaitStop(axis[i]);

                    if (!result.IsSuccess) return result;
                }

                return OperationResult.CreateSuccessResult();

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数长度不正确"
            };

        }

        /// <summary>
        /// 2轴绝对定位
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vel"></param>
        /// <param name="distance"></param>
        /// <param name="velMin"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="sramp"></param>
        /// <returns></returns>
        public OperationResult Move2DAbs(short[] axis, float[] vel, float[] pos, float[] velMin, float[] acc, float[] dec, float[] sramp)
        {
            if (axis.Length == 2 && vel.Length == 2 && pos.Length == 2 && velMin.Length == 2 && acc.Length == 2 && dec.Length == 2 && sramp.Length == 2)
            {
                OperationResult result = new OperationResult();

                //相对定位
                for (int i = 0; i < 2; i++)
                {
                    result = MoveAbs(axis[i], vel[i], pos[i], velMin[i], acc[i], dec[i], sramp[i]);

                    if (!result.IsSuccess) return result;
                }

                //等待停止
                for (int i = 0; i < 2; i++)
                {
                    result = WaitStop(axis[i]);

                    if (!result.IsSuccess) return result;
                }

                return OperationResult.CreateSuccessResult();

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数长度不正确"
            };

        }


        /// <summary>
        /// 3轴相对定位
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vel"></param>
        /// <param name="distance"></param>
        /// <param name="velMin"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="sramp"></param>
        /// <returns></returns>
        public OperationResult Move3DRelative(short[] axis, float[] vel, float[] distance, float[] velMin, float[] acc, float[] dec, float[] sramp)
        {
            if (axis.Length == 3 && vel.Length == 3 && distance.Length == 3 && velMin.Length == 3 && acc.Length == 3 && dec.Length == 3 && sramp.Length == 3)
            {
                OperationResult result = new OperationResult();

                //2轴定位
                result = Move2DRelative(new short[] { axis[0], axis[1] }, new float[] { vel[0], vel[1] }, new float[] { distance[0], distance[1] },

                    new float[] { velMin[0], velMin[1] }, new float[] { acc[0], acc[1] }, new float[] { dec[0], dec[1] }, new float[] { sramp[0], sramp[1] });
                if (!result.IsSuccess) return result;


                result = MoveRelative(axis[2], vel[2], distance[2], velMin[2], acc[2], dec[2], sramp[2]);
                if (!result.IsSuccess) return result;

                //等待停止
                result = WaitStop(axis[2]);
                if (!result.IsSuccess) return result;

                return OperationResult.CreateSuccessResult();
            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数长度不正确"
            };

        }


        /// <summary>
        /// 3轴绝对定位
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vel"></param>
        /// <param name="distance"></param>
        /// <param name="velMin"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="sramp"></param>
        /// <returns></returns>
        public OperationResult Move3DAbs(short[] axis, float[] vel, float[] distance, float[] velMin, float[] acc, float[] dec, float[] sramp)
        {
            if (axis.Length == 3 && vel.Length == 3 && distance.Length == 3 && velMin.Length == 3 && acc.Length == 3 && dec.Length == 3 && sramp.Length == 3)
            {
                OperationResult result = new OperationResult();

                //先动Z轴
                result = MoveAbs(axis[2], vel[2], 0.0f, velMin[2], acc[2], dec[2], sramp[2]);
                if (!result.IsSuccess) return result;

                //等待停止
                result = WaitStop(axis[2]);
                if (!result.IsSuccess) return result;

                //2轴定位
                result = Move2DAbs(new short[] { axis[0], axis[1] }, new float[] { vel[0], vel[1] }, new float[] { distance[0], distance[1] },

                    new float[] { velMin[0], velMin[1] }, new float[] { acc[0], acc[1] }, new float[] { dec[0], dec[1] }, new float[] { sramp[0], sramp[1] });
                if (!result.IsSuccess) return result;


                result = MoveAbs(axis[2], vel[2], distance[2], velMin[2], acc[2], dec[2], sramp[2]);
                if (!result.IsSuccess) return result;

                //等待停止
                result = WaitStop(axis[2]);
                if (!result.IsSuccess) return result;

                return OperationResult.CreateSuccessResult();
            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数长度不正确"
            };

        }


        /// <summary>
        /// 多轴相对直线插补
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public OperationResult MoveLineRelative(int[] axis, float[] distance, float vel, float acc, float dec)
        {

            if (axis.Length >= 2 && axis.Length == distance.Length)
            {
                OperationResult result = new OperationResult();

                //判断每个轴是否满足要求
                foreach (var item in axis)
                {
                    result = CommonMotionValidate((short)item);
                    if (!result.IsSuccess) return result;
                }

                int error = 0;
                try
                {
                    //选择 BASE 轴列表
                    error = zmcaux.ZAux_Direct_Base(Handle, axis.Length, axis);
                    ErrorHandler("ZAux_Direct_Base", error);

                    error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis[0], vel);
                    ErrorHandler("ZAux_Direct_SetSpeed", error);

                    error = zmcaux.ZAux_Direct_SetAccel(Handle, axis[0], acc);
                    ErrorHandler("ZAux_Direct_SetAccel", error);

                    error = zmcaux.ZAux_Direct_SetDecel(Handle, axis[0], dec);
                    ErrorHandler("ZAux_Direct_SetDecel", error);

                    error = zmcaux.ZAux_Direct_Move(Handle, axis.Length, axis, distance);
                    ErrorHandler("ZAux_Direct_Move", error);

                    return OperationResult.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = ex.Message;
                    return result;
                }

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数不正确"
            };
        }

        /// <summary>
        /// 多轴绝对直线插补
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public OperationResult MoveLineAbs(int[] axis, float[] pos, float vel, float acc, float dec)
        {

            if (axis.Length >= 2 && axis.Length == pos.Length)
            {
                OperationResult result = new OperationResult();

                //判断每个轴是否满足要求
                foreach (var item in axis)
                {
                    result = CommonMotionValidate((short)item);
                    if (!result.IsSuccess) return result;
                }

                int error = 0;
                try
                {
                    //选择 BASE 轴列表
                    error = zmcaux.ZAux_Direct_Base(Handle, axis.Length, axis);
                    ErrorHandler("ZAux_Direct_Base", error);

                    error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis[0], vel);
                    ErrorHandler("ZAux_Direct_SetSpeed", error);

                    error = zmcaux.ZAux_Direct_SetAccel(Handle, axis[0], acc);
                    ErrorHandler("ZAux_Direct_SetAccel", error);

                    error = zmcaux.ZAux_Direct_SetDecel(Handle, axis[0], dec);
                    ErrorHandler("ZAux_Direct_SetDecel", error);

                    error = zmcaux.ZAux_Direct_MoveAbs(Handle, axis.Length, axis, pos);
                    ErrorHandler("ZAux_Direct_Move", error);

                    return OperationResult.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = ex.Message;
                    return result;
                }

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数不正确"
            };
        }

        /// <summary>
        /// XY圆弧相对插补(圆心定位)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="circlecenter"></param>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public OperationResult MoveCircleRelative(int[] axis, float[] distance, float[] circlecenter, float vel, float acc, float dec, int dir)
        {

            if (axis.Length == 2 && axis.Length == distance.Length)
            {
                OperationResult result = new OperationResult();

                //判断每个轴是否满足要求
                foreach (var item in axis)
                {
                    result = CommonMotionValidate((short)item);
                    if (!result.IsSuccess) return result;
                }

                int error = 0;
                try
                {
                    //选择 BASE 轴列表
                    error = zmcaux.ZAux_Direct_Base(Handle, axis.Length, axis);
                    ErrorHandler("ZAux_Direct_Base", error);

                    error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis[0], vel);
                    ErrorHandler("ZAux_Direct_SetSpeed", error);

                    error = zmcaux.ZAux_Direct_SetAccel(Handle, axis[0], acc);
                    ErrorHandler("ZAux_Direct_SetAccel", error);

                    error = zmcaux.ZAux_Direct_SetDecel(Handle, axis[0], dec);
                    ErrorHandler("ZAux_Direct_SetDecel", error);

                    error = zmcaux.ZAux_Direct_MoveCirc(Handle, axis.Length, axis, distance[0], distance[1], circlecenter[0], circlecenter[1], dir);
                    ErrorHandler("ZAux_Direct_MoveCirc", error);

                    return OperationResult.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = ex.Message;
                    return result;
                }

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数不正确"
            };
        }


        /// <summary>
        /// XY圆弧绝对插补(圆心定位)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="circlecenter"></param>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public OperationResult MoveCircleAbs(int[] axis, float[] distance, float[] circlecenter, float vel, float acc, float dec, int dir)
        {

            if (axis.Length == 2 && axis.Length == distance.Length)
            {
                OperationResult result = new OperationResult();

                //判断每个轴是否满足要求
                foreach (var item in axis)
                {
                    result = CommonMotionValidate((short)item);
                    if (!result.IsSuccess) return result;
                }

                int error = 0;
                try
                {
                    //选择 BASE 轴列表
                    error = zmcaux.ZAux_Direct_Base(Handle, axis.Length, axis);
                    ErrorHandler("ZAux_Direct_Base", error);

                    error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis[0], vel);
                    ErrorHandler("ZAux_Direct_SetSpeed", error);

                    error = zmcaux.ZAux_Direct_SetAccel(Handle, axis[0], acc);
                    ErrorHandler("ZAux_Direct_SetAccel", error);

                    error = zmcaux.ZAux_Direct_SetDecel(Handle, axis[0], dec);
                    ErrorHandler("ZAux_Direct_SetDecel", error);

                    error = zmcaux.ZAux_Direct_MoveCircAbs(Handle, axis.Length, axis, distance[0], distance[1], circlecenter[0], circlecenter[1], dir);
                    ErrorHandler("ZAux_Direct_MoveCircAbs", error);

                    return OperationResult.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = ex.Message;
                    return result;
                }

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数不正确"
            };
        }

        /// <summary>
        /// XY圆弧相对插补(中点定位)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="midpos"></param>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public OperationResult MoveCircle2Relative(int[] axis, float[] distance, float[] midpos, float vel, float acc, float dec)
        {

            if (axis.Length == 2 && axis.Length == distance.Length)
            {
                OperationResult result = new OperationResult();

                //判断每个轴是否满足要求
                foreach (var item in axis)
                {
                    result = CommonMotionValidate((short)item);
                    if (!result.IsSuccess) return result;
                }

                int error = 0;
                try
                {
                    //选择 BASE 轴列表
                    error = zmcaux.ZAux_Direct_Base(Handle, axis.Length, axis);
                    ErrorHandler("ZAux_Direct_Base", error);

                    error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis[0], vel);
                    ErrorHandler("ZAux_Direct_SetSpeed", error);

                    error = zmcaux.ZAux_Direct_SetAccel(Handle, axis[0], acc);
                    ErrorHandler("ZAux_Direct_SetAccel", error);

                    error = zmcaux.ZAux_Direct_SetDecel(Handle, axis[0], dec);
                    ErrorHandler("ZAux_Direct_SetDecel", error);

                    error = zmcaux.ZAux_Direct_MoveCirc2(Handle, axis.Length, axis, midpos[0], midpos[1], distance[0], distance[1]);
                    ErrorHandler("ZAux_Direct_MoveCirc", error);

                    return OperationResult.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = ex.Message;
                    return result;
                }

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数不正确"
            };
        }


        /// <summary>
        /// XY圆弧绝对插补(中点定位)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="midpos"></param>
        /// <param name="vel"></param>
        /// <param name="acc"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public OperationResult MoveCircle2Abs(int[] axis, float[] distance, float[] midpos, float vel, float acc, float dec)
        {
            if (axis.Length == 2 && axis.Length == distance.Length)
            {
                OperationResult result = new OperationResult();

                //判断每个轴是否满足要求
                foreach (var item in axis)
                {
                    result = CommonMotionValidate((short)item);
                    if (!result.IsSuccess) return result;
                }

                int error = 0;
                try
                {
                    //选择 BASE 轴列表
                    error = zmcaux.ZAux_Direct_Base(Handle, axis.Length, axis);
                    ErrorHandler("ZAux_Direct_Base", error);

                    error = zmcaux.ZAux_Direct_SetSpeed(Handle, axis[0], vel);
                    ErrorHandler("ZAux_Direct_SetSpeed", error);

                    error = zmcaux.ZAux_Direct_SetAccel(Handle, axis[0], acc);
                    ErrorHandler("ZAux_Direct_SetAccel", error);

                    error = zmcaux.ZAux_Direct_SetDecel(Handle, axis[0], dec);
                    ErrorHandler("ZAux_Direct_SetDecel", error);

                    error = zmcaux.ZAux_Direct_MoveCirc2Abs(Handle, axis.Length, axis, midpos[0], midpos[1], distance[0], distance[1]);
                    ErrorHandler("ZAux_Direct_MoveCirc2Abs", error);

                    return OperationResult.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = ex.Message;
                    return result;
                }

            }

            return new OperationResult()
            {
                IsSuccess = false,
                ErrorMsg = "传递参数不正确"
            };
        }


        /// <summary>
        /// 等待停止
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        public OperationResult WaitStop(short axis)
        {
            // 判断是否满足初始化条件
            var result = CommonInitedValidate();

            if (!result.IsSuccess) return result;


            int error = 0;

            int runstate = 0;

            do
            {
                error = zmcaux.ZAux_Direct_GetIfIdle(Handle, axis, ref runstate);

                ErrorHandler("ZAux_Direct_GetIfIdle", error);

            } while (runstate == 0);

            return OperationResult.CreateSuccessResult();
        }


        /// <summary>
        /// 通用回零操作
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="vel">速度</param>
        /// <param name="distance">原限距离</param>
        /// <param name="velMin">最小速度</param>
        /// <param name="acc">加速度</param>
        /// <param name="dec">减速度</param>
        /// <param name="sramp">s曲线时间</param>
        /// <returns>操作结果</returns>
        public OperationResult ZeroAxis(short axis, float vel, float distance, float velMin, float acc, float dec, float sramp)
        {
            // 判断是否满足运动条件
            var result = CommonMotionValidate(axis);

            if (!result.IsSuccess) return result;

            //先往原点方向一直走，直到到达限位
            result = MoveRelative(axis, vel, -5000, velMin, acc, dec, sramp);
            if (!result.IsSuccess) return result;

            //等待停止
            result = WaitStop(axis);
            if (!result.IsSuccess) return result;

            //往反向走一段距离，超过原点和限位的距离
            result = MoveRelative(axis, vel, distance, velMin, acc, dec, sramp);
            if (!result.IsSuccess) return result;

            //等待停止
            result = WaitStop(axis);
            if (!result.IsSuccess) return result;

            //返回成功
            return OperationResult.CreateSuccessResult();

        }


        /// <summary>
        /// 停止轴
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>操作结果</returns>
        public OperationResult StopAxis(short axis)
        {
            var result = CommonInitedValidate();

            if (!result.IsSuccess) return result;

            //错误码
            int error = 0;

            try
            {
                /*         
                0 （缺省）取消当前运动
                1 取消缓冲的运动
                2 取消当前运动和缓冲运动
                3 立即中断脉冲发送
                 */

                error = zmcaux.ZAux_Direct_Single_Cancel(Handle, axis, 2);
                ErrorHandler("ZAux_Direct_Single_Cancel", error);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }


        /// <summary>
        /// 停止所有轴
        /// </summary>
        /// <returns></returns>
        public OperationResult StopAllAxis()
        {
            var result = CommonInitedValidate();

            if (!result.IsSuccess) return result;

            //错误码
            int error = 0;

            try
            {
                /*         
                    0取消当前运动 。
                    1取消缓冲的运动 。
                    2取消当前运动和缓冲运动 。
                    3 立即停止。

                 */

                error = zmcaux.ZAux_Direct_CancelAxisList(Handle, 4, new int[] { 0, 1, 2, 3 }, 3);
                ErrorHandler("ZAux_Direct_CancelAxisList", error);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return result;
            }
            return OperationResult.CreateSuccessResult();

        }

        /// <summary>
        /// 获取实时速度
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetVel(short axis)
        {
            //判断是否满足初始化条件
            var result = CommonInitedValidate();

            if (!result.IsSuccess) return 0.0f;

            //定义速度
            float vel = 0.0f;

            //定义错误码
            int error = 0;

            try
            {
                error = zmcaux.ZAux_Direct_GetVpSpeed(Handle, axis, ref vel);

                ErrorHandler("ZAux_Direct_GetVpSpeed", error);

                return vel;

            }
            catch (Exception)
            {
                return 0.0f;
            }
        }


        /// <summary>
        /// 获取实时位置
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetPos(short axis)
        {

            //判断是否满足初始化条件
            var result = CommonInitedValidate();

            if (!result.IsSuccess) return 0.0f;

            //定义位置
            float pos = 0.0f;

            //定义错误码
            int error = 0;

            try
            {

                error = zmcaux.ZAux_Direct_GetMpos(Handle, axis, ref pos);

                ErrorHandler("ZAux_Direct_GetMpos", error);

                return pos;

            }
            catch (Exception)
            {
                return 0.0f;
            }
        }


        /// <summary>
        /// 位置清零
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public OperationResult ZeroPos(short axis)
        {
            //判断是否满足初始化条件
            var result = CommonInitedValidate();

            if (!result.IsSuccess) return result;

            //定义错误码
            int error = 0;

            try
            {

                error = zmcaux.ZAux_Direct_SetMpos(Handle, axis, 0.0f);

                ErrorHandler("ZAux_Direct_SetMpos", error);

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }


        /// <summary>
        /// 通用运动验证
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>操作结果</returns>
        private OperationResult CommonMotionValidate(short axis)
        {
            OperationResult result = CommonInitedValidate();
            //判断是否已经初始化
            if (!result.IsSuccess) return result;

            //判断是否正在运行
            if (IsMoving(axis))
            {
                result.IsSuccess = false;
                result.ErrorMsg = "轴正在运行";
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }


        /// <summary>
        /// 通用初始化验证
        /// </summary>
        /// <returns></returns>
        private OperationResult CommonInitedValidate()
        {
            OperationResult result = new OperationResult();
            //判断是否已经初始化
            if (!InitedOK)
            {
                result.IsSuccess = false;
                result.ErrorMsg = "控制器未初始化";
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }


        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="command">执行命令</param>
        /// <param name="error">错误码</param>
        private void ErrorHandler(string command, int error)
        {
            string result = string.Empty;
            switch (error)
            {
                case 0: break;
                default:
                    result = string.Format("{0}" + "指令执行错误，错误码为{1}", command, error);
                    break;
            }
            if (result.Length > 0)
            {
                throw new Exception(result);
            }
        }

        /// <summary>
        /// 判断某个轴是否正在运行
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool IsMoving(short axis)
        {
            OperationResult result = CommonInitedValidate();
            //判断是否已经初始化
            if (!result.IsSuccess) return false;
            //定义运行状态
            int runstate = -1;
            //定义错误码
            int error = 0;
            try
            {
                //获取轴状态
                error = zmcaux.ZAux_Direct_GetIfIdle(Handle, axis, ref runstate);
                //错误码验证
                ErrorHandler("ZAux_Direct_GetIfIdle", error);
                return runstate == 0;
            }
            catch (Exception)
            {
                return true;
            }
        }


        #region 根据输入索引获取位
        /// <summary>
        /// 根据索引获取位
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>位结果</returns>
        public bool GetInput(short index)
        {
            //是否初始化
            OperationResult result = CommonInitedValidate();

            if (!result.IsSuccess) return false;

            int error = 0;

            uint res = 0;
            try
            {
                error = zmcaux.ZAux_Direct_GetIn(Handle, index, ref res);
                ErrorHandler("ZAux_Direct_GetIn", error);
                return res == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 根据索引获取位
        /// <summary>
        /// 根据索引获取位
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>位结果</returns>
        public bool GetOutput(short index)
        {
            //是否初始化
            OperationResult result = CommonInitedValidate();

            if (!result.IsSuccess) return false;

            int error = 0;

            uint res = 0;
            try
            {
                error = zmcaux.ZAux_Direct_GetOp(Handle, index, ref res);
                ErrorHandler("ZAux_Direct_GetOp", error);
                return res == 1;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion

        #region 根据索引操作位

        /// <summary>
        /// 根据索引操作位
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="open">操作位</param>
        /// <returns>操作结果</returns>
        public OperationResult SetOutput(short index, bool open)
        {
            //是否初始化
            OperationResult result = CommonInitedValidate();

            if (!result.IsSuccess) return result;

            int error = 0;
            try
            {
                error = zmcaux.ZAux_Direct_SetOp(Handle, index, open ? (uint)1 : 0);

                ErrorHandler("ZAux_Direct_SetOp", error);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return result;
            }
            return OperationResult.CreateSuccessResult();
        }

        #endregion

    }



}
