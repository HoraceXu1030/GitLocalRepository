using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.VisionModels;

namespace thinger.VisionDAL
{
    public class ImageHelper
    {
        public ImageHelper(string CamName = "A7500-CG20-A", string DrvName = "GigEVision2")
        {
            this.CamName = CamName;
            this.DrvName = DrvName;
        }

        private string CamName = "A7500-CG20-A";

        private string DrvName = "GigEVision2";

        //相机对象句柄
        private HTuple acqHandle;

        public bool isCamOK = false;

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="CamName"></param>
        /// <param name="Drvname"></param>
        /// <returns></returns>
        public OperationResult OpenCam()
        {
            try
            {
                HOperatorSet.CloseAllFramegrabbers();

                HOperatorSet.OpenFramegrabber(DrvName, 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", CamName, 0, -1, out acqHandle);

                isCamOK = true;

                return OperationResult.CreateSuccessResult();


            }
            catch (Exception ex)
            {
                isCamOK = false;

                acqHandle = null;

                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <returns></returns>
        public OperationResult CloseCam()
        {
            try
            {
                if (acqHandle != null)
                {
                    HOperatorSet.CloseFramegrabber(acqHandle);
                    acqHandle = null;
                }
                return OperationResult.CreateSuccessResult();

            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }



        /// <summary>
        /// 获取曝光时间
        /// </summary>
        /// <returns></returns>
        public int GetExposureTime()
        {
            try
            {
                if (acqHandle != null)
                {
                    HOperatorSet.GetFramegrabberParam(acqHandle, "ExposureTime", out HTuple value);

                    if (value != null && value.Length > 0)
                    {
                        return value.TupleInt();

                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationResult SetExposureTime(HTuple value)
        {
            try
            {
                if (acqHandle != null)
                {
                    HOperatorSet.SetFramegrabberParam(acqHandle, "ExposureTime", value);
                }
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            return OperationResult.CreateSuccessResult();

        }


        /// <summary>
        /// 获取增益值
        /// </summary>
        /// <returns></returns>
        public int GetGain()
        {
            try
            {
                if (acqHandle != null)
                {
                    HOperatorSet.GetFramegrabberParam(acqHandle, "Gain", out HTuple value);

                    if (value != null && value.Length > 0)
                    {
                        return value.TupleInt();

                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationResult SetGain(HTuple value)
        {
            try
            {
                if (acqHandle != null)
                {
                    HOperatorSet.SetFramegrabberParam(acqHandle, "Gain", value);
                }
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            return OperationResult.CreateSuccessResult();
        }

        /// <summary>
        /// 获取触发模式
        /// </summary>
        /// <returns></returns>
        public bool GetTriggerMode()
        {
            try
            {
                if (acqHandle != null)
                {
                    HOperatorSet.GetFramegrabberParam(acqHandle, "TriggerMode", out HTuple value);

                    if (value != null && value.Length > 0)
                    {
                        return value.S == "On";

                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 设置触发模式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationResult SetTriggerMode(bool value)
        {
            try
            {
                if (acqHandle != null)
                {
                    HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerMode", value ? "On" : "Off");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
            return OperationResult.CreateSuccessResult();
        }

        /// <summary>
        /// 采集图像
        /// </summary>
        /// <param name="hWindowHandle"></param>
        /// <param name="hImage"></param>
        /// <returns></returns>
        public OperationResult GrabImage(ref HTuple hWindowHandle, ref HObject hImage)
        {
            try
            {
                HOperatorSet.GrabImage(out hImage, acqHandle);

                //显示全图
                HOperatorSet.GetImageSize(hImage, out HTuple width, out HTuple height);

                //设置窗体句柄的大小
                HOperatorSet.SetPart(hWindowHandle, 0, 0, height - 1, width - 1);

                //显示
                HOperatorSet.DispObj(hImage, hWindowHandle);

                return OperationResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }




        /// <summary>
        /// 读取图像并显示在窗体中
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hWindowHandle"></param>
        /// <param name="hImage"></param>
        /// <returns></returns>
        public OperationResult ReadImage(string path, ref HTuple hWindowHandle, ref HObject hImage)
        {
            try
            {
                //根据路径读取图像
                HOperatorSet.ReadImage(out hImage, path);

                //显示全图
                HOperatorSet.GetImageSize(hImage, out HTuple width, out HTuple height);

                //设置窗体句柄的大小
                HOperatorSet.SetPart(hWindowHandle, 0, 0, height - 1, width - 1);

                //显示
                HOperatorSet.DispObj(hImage, hWindowHandle);

                return OperationResult.CreateSuccessResult();

            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hImage"></param>
        /// <returns></returns>
        public OperationResult SaveImage(string path, string format, HObject hImage)
        {
            try
            {
                HOperatorSet.WriteImage(hImage, format, 0, path);

                return OperationResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }



    }
}
