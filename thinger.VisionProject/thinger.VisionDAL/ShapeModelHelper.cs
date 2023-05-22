using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.VisionModels;

namespace thinger.VisionDAL
{
    public class ShapeModelHelper
    {

        private HalconHelper halcon = new HalconHelper();

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="hWindowHandle"></param>
        /// <param name="hImage"></param>
        /// <param name="matchParams"></param>
        /// <returns></returns>
        public OperationResult CreateShapeModel(HTuple hWindowHandle, HObject hImage, MatchParams matchParams)
        {
            try
            {
                //提示
                halcon.disp_message(hWindowHandle, "请绘制模板区域，完成后鼠标右击确认", "window", 20, 20, "red", "false");

                //Draw
                HOperatorSet.DrawRectangle2(hWindowHandle, out HTuple row, out HTuple column, out HTuple phi, out HTuple length1, out HTuple length2);

                //Gen
                HOperatorSet.GenRectangle2(out matchParams.modelRegion, row, column, phi, length1, length2);

                //ReduceDomain
                HOperatorSet.ReduceDomain(hImage, matchParams.modelRegion, out HObject imageReduced);

                //Clear
                ClearShapeMode(ref matchParams.modelId);

                //Create
                HOperatorSet.CreateShapeModel(imageReduced, "auto", matchParams.startAngle, matchParams.rangeAngle, "auto", "auto", "use_polarity", "auto", "auto", out matchParams.modelId);

                HOperatorSet.DispObj(hImage, hWindowHandle);

                HOperatorSet.DispObj(matchParams.modelRegion, hWindowHandle);


                if (matchParams.modelId != null)
                {
                    //提示
                    halcon.disp_message(hWindowHandle, "创建模板成功", "window", 20, 20, "green", "false");

                    return OperationResult.CreateSuccessResult();
                }
                else
                {
                    //提示
                    halcon.disp_message(hWindowHandle, "创建模板失败", "window", 20, 20, "red", "false");

                    return OperationResult.CreateFailResult();
                }
            }
            catch (Exception ex)
            {
                //提示
                halcon.disp_message(hWindowHandle, "创建模板失败：" + ex.Message, "window", 20, 20, "red", "false");

                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }


        /// <summary>
        /// 清除模板
        /// </summary>
        /// <param name="ModelId"></param>
        public void ClearShapeMode(ref HTuple ModelId)
        {
            if (ModelId != null)
            {
                HOperatorSet.ClearShapeModel(ModelId);
                ModelId = null;
            }
        }



        /// <summary>
        /// 查找模板
        /// </summary>
        /// <param name="hWindowHandle"></param>
        /// <param name="hImage"></param>
        /// <param name="matchParams"></param>
        /// <param name="homMat2D"></param>
        /// <returns></returns>
        public OperationResult FindShapeModel(HTuple hWindowHandle, HObject hImage, MatchParams matchParams, out HTuple homMat2D)
        {
            homMat2D = new HTuple();

            try
            {
                HOperatorSet.FindShapeModel(hImage, matchParams.modelId, matchParams.startAngle, matchParams.rangeAngle, matchParams.score,

                               matchParams.numMatchs, matchParams.overlap, "least_squares", 0, matchParams.greediness, out HTuple row, out HTuple column, out HTuple angle, out HTuple score);


                //通过判断结果，证明是否查找到模板
                if (row.Length > 0)
                {
                    //处理

                    //显示轮廓

                    //获取仿射变化矩阵

                    HOperatorSet.VectorAngleToRigid(0, 0, 0, row, column, angle, out HTuple homMat2d_T);

                    //获取轮廓
                    HOperatorSet.GetShapeModelContours(out HObject contour, matchParams.modelId, 1);

                    //将显示在0.0 位置的轮廓仿射过去并显示

                    HOperatorSet.AffineTransContourXld(contour, out HObject contour2, homMat2d_T);

                    //产生一个十字叉
                    HOperatorSet.GenCrossContourXld(out HObject cross, row, column, 20, new HTuple(45));


                    HOperatorSet.AreaCenter(matchParams.modelRegion, out HTuple area, out HTuple row0, out HTuple column0);

                    //获取圆ROI的仿射变化矩阵

                    HOperatorSet.VectorAngleToRigid(row0, column0, 0, row, column, angle, out homMat2D);

                    //显示
                    HOperatorSet.DispObj(hImage, hWindowHandle);

                    HOperatorSet.DispObj(contour, hWindowHandle);

                    HOperatorSet.DispObj(contour2, hWindowHandle);

                    HOperatorSet.DispObj(cross, hWindowHandle);

                    //提示
                    halcon.disp_message(hWindowHandle, "查找模板成功", "window", 20, 20, "green", "false");

                    return new OperationResult()
                    {
                        IsSuccess = true,
                        ErrorMsg = "查找模板成功"
                    };
                }
                else
                {

                    //提示
                    halcon.disp_message(hWindowHandle, "查找模板失败", "window", 20, 20, "red", "false");


                    return new OperationResult()
                    {
                        IsSuccess = false,
                        ErrorMsg = "未查找到模板"
                    };
                }
            }
            catch (Exception ex)
            {
                //提示
                halcon.disp_message(hWindowHandle, "查找模板失败：" + ex.Message, "window", 20, 20, "red", "false");

                return new OperationResult()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message
                };
            }
        }



    }
}
