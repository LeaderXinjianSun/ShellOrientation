
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
        public static void CalcOpeningRec1(HObject ho_image, HObject ho_Rectangle, HTuple ThresholdMin, HTuple ThresholdMax, HTuple OpeningRec1Width, HTuple OpeningRec1Height, HTuple GapMax,
            out HObject ho_SelectedRegions, out HTuple hv_isOK)
        {
            // Local iconic variables 
            HObject ho_ImageReduced, ho_GrayImage, ho_Region, ho_RegionFillUp, image2, image3, ho_ConnectedRegions;
            HObject ho_RegionOpening0, ho_RegionOpening, ho_RegionDifference;

            // Local control variables 

            HTuple hv_widths = null, hv_maxWidth = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out image2);
            HOperatorSet.GenEmptyObj(out image3);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening0);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_image, ho_Rectangle, out ho_ImageReduced);
            ho_GrayImage.Dispose(); image2.Dispose(); image3.Dispose();
            //HOperatorSet.Rgb1ToGray(ho_ImageReduced, out ho_GrayImage);
            HOperatorSet.Decompose3(ho_ImageReduced, out ho_GrayImage, out image2, out image3);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_GrayImage, out ho_Region, ThresholdMin, ThresholdMax);

            ho_RegionOpening0.Dispose();
            HOperatorSet.OpeningRectangle1(ho_Region, out ho_RegionOpening0, OpeningRec1Width, 20);//把毛刺先滤除一遍。轨道发白

            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_RegionOpening0, out ho_RegionFillUp);

            ho_RegionOpening.Dispose();
            HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpening, OpeningRec1Width, OpeningRec1Height);

            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_RegionFillUp, ho_RegionOpening, out ho_RegionDifference
                );
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionDifference, out ho_ConnectedRegions);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, new HTuple("area").TupleConcat("height"), "and", new HTuple(200).TupleConcat(50), new HTuple(999999).TupleConcat(999999));//把细小的干扰滤除掉。太窄的也去掉

            HOperatorSet.RegionFeatures(ho_SelectedRegions, "width", out hv_widths);

            if (hv_widths.TupleLength() > 0)
            {
                hv_maxWidth = hv_widths.TupleMax();
                hv_isOK = 1;
                if (new HTuple(hv_maxWidth.TupleGreater(GapMax)) != 0)
                {
                    hv_isOK = 0;
                }
            }
            else
            {
                hv_isOK = 1;
            }
            ho_ImageReduced.Dispose();
            ho_GrayImage.Dispose(); image2.Dispose(); image3.Dispose();
            ho_Region.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionOpening0.Dispose();
            ho_RegionOpening.Dispose();
            ho_RegionDifference.Dispose();
            ho_ConnectedRegions.Dispose();
        }
        public static void SubImage(HObject image1,HObject iamgeSTX, HObject executeRec1,out HObject ho_resultRegion,out HTuple hv_result)
        {
            HTuple executeRec1Row1;
            HOperatorSet.RegionFeatures(executeRec1, "row1", out executeRec1Row1);
            HTuple executeRec1Column1;
            HOperatorSet.RegionFeatures(executeRec1, "column1", out executeRec1Column1);
            HTuple executeRec1Width;
            HOperatorSet.RegionFeatures(executeRec1, "width", out executeRec1Width);
            HTuple executeRec1Height;
            HOperatorSet.RegionFeatures(executeRec1, "height", out executeRec1Height);
            HObject red1, red2;
            HObject imagePart;
            HOperatorSet.CropPart(image1, out imagePart, executeRec1Row1, executeRec1Column1, executeRec1Width, executeRec1Height);
            HOperatorSet.Rgb1ToGray(imagePart, out red1);

            HOperatorSet.Rgb1ToGray(iamgeSTX, out red2);
            HObject imageSub;
            HOperatorSet.SubImage(red2, red1, out imageSub, 1, 128);
            HObject ho_Region;
            HOperatorSet.Threshold(imageSub, out ho_Region, new HTuple(0).TupleConcat(200), new HTuple(50).TupleConcat(255));
            HObject ho_RegionUnion;
            HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
            HTuple HomMat2D;
            HOperatorSet.HomMat2dIdentity(out HomMat2D);
            HOperatorSet.HomMat2dTranslate(HomMat2D, executeRec1Row1, executeRec1Column1, out HomMat2D);
            HOperatorSet.AffineTransRegion(ho_RegionUnion, out ho_resultRegion, HomMat2D, "nearest_neighbor");

            HTuple hv_union_area;
            HOperatorSet.RegionFeatures(ho_RegionUnion, "area", out hv_union_area);

            HTuple image_width, iamge_height;
            HOperatorSet.GetImageSize(red1, out image_width, out iamge_height);
            if (hv_union_area / (image_width * iamge_height) > 0.3)
            {
                hv_result = new HTuple(0);
            }
            else
            {
                hv_result = new HTuple(1);
            }

            imageSub.Dispose();
            ho_Region.Dispose();
            ho_RegionUnion.Dispose();
            imagePart.Dispose();
            red1.Dispose();
            red2.Dispose();
        }
    }
}
