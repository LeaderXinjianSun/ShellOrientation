
using HalconDotNet;
using System;

namespace ShellOrientation.Models
{
    public class ImageCalc
    {
        // Procedures 
        public static void Calc1(HObject ho_image, HObject ho_region, out HObject ho_hv_resultRegion,
            HTuple hv_maxgray, HTuple hv_shapemin, HTuple hv_shapemax, HTuple hv_length1min,
            HTuple hv_length1max, out HTuple hv_hv_result)
        {




            // Local iconic variables 

            HObject ho_Red = null, ho_Green = null, ho_Blue = null;
            HObject ho_ImageReduced, ho_Region, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionFillUp;

            // Local control variables 

            HTuple hv_Channels = null, hv_Row1 = null;
            HTuple hv_Column1 = null, hv_Phi1 = null, hv_Length11 = null;
            HTuple hv_Length21 = null, hv_Deg = new HTuple(), hv_DegAbs = new HTuple();
            HTuple hv_Min = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_hv_resultRegion);
            HOperatorSet.GenEmptyObj(out ho_Red);
            HOperatorSet.GenEmptyObj(out ho_Green);
            HOperatorSet.GenEmptyObj(out ho_Blue);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            hv_hv_result = new HTuple();
            HOperatorSet.CountChannels(ho_image, out hv_Channels);
            if ((int)(new HTuple(hv_Channels.TupleEqual(3))) != 0)
            {
                ho_Red.Dispose(); ho_Green.Dispose(); ho_Blue.Dispose();
                HOperatorSet.Decompose3(ho_image, out ho_Red, out ho_Green, out ho_Blue);
            }
            else
            {
                ho_Red.Dispose();
                ho_Red = ho_image.CopyObj(1, -1);
            }

            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Red, ho_region, out ho_ImageReduced);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, hv_maxgray);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("area")).TupleConcat(
                "rect2_len1"), "and", hv_shapemin.TupleConcat(hv_length1min), hv_shapemax.TupleConcat(
                hv_length1max));
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
            HOperatorSet.SmallestRectangle2(ho_RegionFillUp, out hv_Row1, out hv_Column1,
                out hv_Phi1, out hv_Length11, out hv_Length21);
            if ((int)(new HTuple((new HTuple(hv_Phi1.TupleLength())).TupleGreater(0))) != 0)
            {
                ho_hv_resultRegion.Dispose();
                HOperatorSet.GenRectangle2(out ho_hv_resultRegion, hv_Row1, hv_Column1, hv_Phi1,
                    hv_Length11, hv_Length21);
                HOperatorSet.TupleDeg(hv_Phi1, out hv_Deg);
                HOperatorSet.TupleAbs(hv_Deg, out hv_DegAbs);
                HOperatorSet.TupleMin(hv_DegAbs, out hv_Min);
                if ((int)(new HTuple(hv_Min.TupleLess(30))) != 0)
                {
                    hv_hv_result = 0;
                }
                else
                {
                    hv_hv_result = 1;
                }
            }
            else
            {
                hv_hv_result = 1;
            }

            ho_Red.Dispose();
            ho_Green.Dispose();
            ho_Blue.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp.Dispose();

            return;
        }
    }
}
