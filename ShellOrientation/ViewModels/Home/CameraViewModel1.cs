

using HalconDotNet;
using Newtonsoft.Json;
using NLog;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using ShellOrientation.Common.Extensions;
using ShellOrientation.Common.Services;
using ShellOrientation.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ViewROI;

namespace ShellOrientation.ViewModels.Home
{
    public class CameraViewModel1 : BindableBase
    {
        #region 变量
        ICameraService cam;
        IPLCModbusService plc;
        public readonly IEventAggregator aggregator;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CancellationTokenSource source;
        Param param;
        #endregion
        #region 属性
        #region halcon
        #region 1#
        private HImage cameraIamge0;
        public HImage CameraIamge0
        {
            get { return cameraIamge0; }
            set { SetProperty(ref cameraIamge0, value); }
        }
        private bool cameraRepaint0;
        public bool CameraRepaint0
        {
            get { return cameraRepaint0; }
            set { SetProperty(ref cameraRepaint0, value); }
        }
        private ObservableCollection<ROI> cameraROIList0 = new ObservableCollection<ROI>();
        public ObservableCollection<ROI> CameraROIList0
        {
            get { return cameraROIList0; }
            set { SetProperty(ref cameraROIList0, value); }
        }
        private HObject cameraAppendHObject0;
        public HObject CameraAppendHObject0
        {
            get { return cameraAppendHObject0; }
            set { SetProperty(ref cameraAppendHObject0, value); }
        }
        private HMsgEntry cameraAppendHMessage0;
        public HMsgEntry CameraAppendHMessage0
        {
            get { return cameraAppendHMessage0; }
            set { SetProperty(ref cameraAppendHMessage0, value); }
        }
        private Tuple<string, object> cameraGCStyle0;
        public Tuple<string, object> CameraGCStyle0
        {
            get { return cameraGCStyle0; }
            set { SetProperty(ref cameraGCStyle0, value); }
        }
        #endregion
        #endregion
        #endregion
        #region 构造函数
        public CameraViewModel1(IContainerProvider containerProvider, IEventAggregator _aggregator)
        {
            aggregator = _aggregator;
            LoadParam();
            cam = containerProvider.Resolve<ICameraService>("Cam1");
            plc = containerProvider.Resolve<IPLCModbusService>("plc1");
            var r = cam.OpenCamera(param.Camera1Name, "DirectShow");//[0] Integrated Camera //[1] LRCP  USB2.0
            //M800-M803
            if (r)
            {
                cam.GrabeImageStart();
                aggregator.SendMessage("Camera1OpenOK", "Camera");
                source = new CancellationTokenSource();
                CancellationToken token1 = source.Token;
                Task.Run(() => Run(token1), token1);
            }
            else
            {
                aggregator.SendMessage("Camera1OpenNG", "Camera");
            }
            aggregator.ResgiterMessage(arg => {
                switch (arg.Message)
                {
                    case "Closed":
                        if (source != null)
                        {
                            source.Cancel();
                        }
                        break;
                    default:
                        break;
                }
            }, "App");
        }
        #endregion
        #region 功能函数
        private void LoadParam()
        {
            //Json序列化，从文件读取
            string jsonString = File.ReadAllText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
            param = JsonConvert.DeserializeObject<Param>(jsonString);
        }
        private void Run(CancellationToken token)
        {
            string filepath = $"Camera\\1";
            
            HTuple RotateDeg;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "RotateDeg.tup"), out RotateDeg);
            HTuple thresholdMin;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin.tup"), out thresholdMin);
            HTuple thresholdMax;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax.tup"), out thresholdMax);
            HTuple OpeningRec1Width;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width.tup"), out OpeningRec1Width);
            HTuple OpeningRec1Height;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height.tup"), out OpeningRec1Height);
            HTuple GapMax;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax.tup"), out GapMax);

            HTuple thresholdMin_2;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin_2.tup"), out thresholdMin_2);
            HTuple thresholdMax_2;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax_2.tup"), out thresholdMax_2);
            HTuple OpeningRec1Width_2;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width_2.tup"), out OpeningRec1Width_2);
            HTuple OpeningRec1Height_2;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height_2.tup"), out OpeningRec1Height_2);
            HTuple GapMax_2;
            HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax_2.tup"), out GapMax_2);
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    HObject ho_ImageRotate;
                    HOperatorSet.RotateImage(cam.GrabeImageAsync(), out ho_ImageRotate, RotateDeg, "constant");
                    CameraIamge0 = new HImage(ho_ImageRotate);

                    HObject rec1_0, rec1_1;
                    HOperatorSet.ReadRegion(out rec1_0, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_0.hobj"));
                    HOperatorSet.ReadRegion(out rec1_1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_1.hobj"));
                    HTuple hv_result; HObject hv_resultRegion1;
                    ImageCalc.CalcOpeningRec1(ho_ImageRotate, rec1_0, thresholdMin, thresholdMax, OpeningRec1Width, OpeningRec1Height, GapMax, out hv_resultRegion1, out hv_result);
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_result == 1)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                        }
                        CameraAppendHObject0 = null;
                        CameraAppendHObject0 = hv_resultRegion1;
                    }));
                    plc.WriteMCoil(800,!(hv_result == 1));

                    HObject hv_resultRegion2;
                    ImageCalc.CalcOpeningRec1(ho_ImageRotate, rec1_1, thresholdMin_2, thresholdMax_2, OpeningRec1Width_2, OpeningRec1Height_2, GapMax_2, out hv_resultRegion2, out hv_result);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_result == 1)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                        }
                        CameraAppendHObject0 = hv_resultRegion2;
                    }));
                    plc.WriteMCoil(801, !(hv_result == 1));
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(100);
            }
        }
        #endregion
    }
}
