
using HalconDotNet;

namespace ShellOrientation.Models
{
    public class ImageCalc
    {
        public static void Calc1(HObject img,HObject line,ref int hv_Result,ref HObject ho_ResultRegion)
        {
            HObject ImageReduced;
            HOperatorSet.ReduceDomain(img, line, out ImageReduced);
            HObject Region1;
            HOperatorSet.Threshold(ImageReduced, out Region1, 150, 255);
            HObject ConnectedRegions;
            HOperatorSet.Connection(Region1, out ConnectedRegions);
            HObject SelectedRegions;
            HOperatorSet.SelectShape(ConnectedRegions, out SelectedRegions, "area","and", 30, 999);
            HTuple hv_Area;
            HOperatorSet.AreaCenter(SelectedRegions, out hv_Area, out _, out _);
            if (new HTuple(new HTuple(hv_Area.TupleLength()).TupleGreater(0)) != 0)
            {
                HTuple hv_Min;
                HOperatorSet.TupleMax(hv_Area, out hv_Min);
                if (new HTuple(hv_Min.TupleLess(60)) != 0)
                {
                    hv_Result = 0;
                }
                else
                {
                    hv_Result = 1;
                }
            }
            else
            {
                hv_Result = 1;
            }
            ho_ResultRegion.Dispose();
            ho_ResultRegion = SelectedRegions.CopyObj(1, -1);
            ConnectedRegions.Dispose();
            SelectedRegions.Dispose();
        }
    }
}
