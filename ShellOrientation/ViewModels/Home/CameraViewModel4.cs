

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
    public class CameraViewModel4 : BindableBase
    {
        #region 变量
        ICameraService cam;
        IPLCModbusService plc;
        public readonly IEventAggregator aggregator;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CancellationTokenSource source;
        Param param;
        bool plcConnect = false;
        HTuple RotateDeg, thresholdMin, thresholdMax, OpeningRec1Width, OpeningRec1Height, GapMax, thresholdMin_2, thresholdMax_2, OpeningRec1Width_2, OpeningRec1Height_2,
GapMax_2, IsExcludeRobotMove, MussyWidth, MussyHeight, diffShapeArea, diffShapeHeight, MussyWidth_2, MussyHeight_2, diffShapeArea_2, diffShapeHeight_2;
        HObject iamgeSTX, executeRec1;
        HObject rec1_0, rec1_1;
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
        public CameraViewModel4(IContainerProvider containerProvider, IEventAggregator _aggregator)
        {
            aggregator = _aggregator;
            LoadParam();
            cam = containerProvider.Resolve<ICameraService>("Cam4");
            plc = containerProvider.Resolve<IPLCModbusService>("plc2");
            var r = cam.OpenCamera(param.Camera4Name, "DirectShow");//[0] Integrated Camera //[1] LRCP  USB2.0
            //M800-M803
            if (r)
            {
                aggregator.SendMessage("Camera4OpenOK", "Camera");
                LoadCameraParm();
                source = new CancellationTokenSource();
                CancellationToken token1 = source.Token;
                Task.Run(() => Run(token1), token1);
            }
            else
            {
                aggregator.SendMessage("Camera4OpenNG", "Camera");
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
                    case "SaveImage":
                        if (r)
                        {
                            try
                            {
                                string filepath = $"SaveImage\\4";
                                DirectoryInfo dir = new DirectoryInfo(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
                                if (!dir.Exists)
                                {
                                    Directory.CreateDirectory(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
                                }
                                HOperatorSet.WriteImage(CameraIamge0, "jpeg", 0, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, $"4_{DateTime.Now:yyyyMMdd_HHmmss}.jpg"));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;
                    case "PLC2Connect":
                        plcConnect = true;
                        break;
                    case "ReloadCamera4Param":
                        {
                            LoadCameraParm();
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
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    var m808 = plc.ReadMCoils(808, 2);
                    if (m808 != null)
                    {
                        if (m808[0] || m808[1])
                        {
                            plc.WriteMCoil(808, false);
                            plc.WriteMCoil(809, false);
                            var img = cam.GrabImage();
                            if (img == null)
                            {
                                aggregator.SendMessage("Camera4OpenNG", "Camera");
                                cam.CloseCamera();
                                Thread.Sleep(1000);
                                var r = cam.OpenCamera(param.Camera4Name, "DirectShow");
                                if (r)
                                {
                                    cam.GrabeImageStart();
                                    aggregator.SendMessage("Camera4OpenOK", "Camera");
                                    logger.Info("④相机重连:成功");
                                }
                                else
                                {
                                    logger.Info("④相机重连:失败");
                                }
                                continue;
                            }
                            HObject ho_ImageRotate;
                            HOperatorSet.RotateImage(img, out ho_ImageRotate, RotateDeg, "constant");
                            CameraIamge0 = new HImage(ho_ImageRotate);

                            if (IsExcludeRobotMove.I == 1)
                            {

                                HObject ho_resultRegion;
                                HTuple hv_result0;
                                ImageCalc.SubImage(CameraIamge0, iamgeSTX, executeRec1, out ho_resultRegion, out hv_result0);

                                if (hv_result0.I == 0)
                                {
                                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        CameraGCStyle0 = new Tuple<string, object>("DrawMode", "fill");
                                        CameraGCStyle0 = new Tuple<string, object>("Color", "orange red");
                                        CameraAppendHObject0 = null;
                                        CameraAppendHObject0 = ho_resultRegion;
                                        CameraAppendHMessage0 = null;
                                        CameraAppendHMessage0 = new HMsgEntry("画面有干扰，不做计算。", 10, 10, "red", "window", "box", "false", 32, "mono", "true", "false");
                                    }));
                                    if (plcConnect)
                                    {
                                        plc.WriteMCoil(802, false);
                                        plc.WriteMCoil(803, false);
                                    }
                                    Thread.Sleep(100);
                                    continue;
                                }
                            }
                            HTuple hv_result; HObject hv_resultRegion1;
                            ImageCalc.CalcOpeningRec1(ho_ImageRotate, rec1_0, thresholdMin, thresholdMax, OpeningRec1Width, OpeningRec1Height, GapMax, MussyWidth, MussyHeight, diffShapeArea, diffShapeHeight, out hv_resultRegion1, out hv_result);
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                if (hv_result == 1)
                                {
                                    CameraAppendHMessage0 = new HMsgEntry("1:OK", 10, 10, "green", "window", "box", "false", 32, "mono", "true", "false");
                                    CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                                }
                                else
                                {
                                    CameraAppendHMessage0 = new HMsgEntry("1:NG", 10, 10, "red", "window", "box", "false", 32, "mono", "true", "false");
                                    CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                                }
                                CameraAppendHObject0 = hv_resultRegion1;
                            }));
                            if (plcConnect)
                                plc.WriteMCoil(802, !(hv_result == 1));

                            HObject hv_resultRegion2;
                            ImageCalc.CalcOpeningRec1(ho_ImageRotate, rec1_1, thresholdMin_2, thresholdMax_2, OpeningRec1Width_2, OpeningRec1Height_2, GapMax_2, MussyWidth_2, MussyHeight_2, diffShapeArea_2, diffShapeHeight_2, out hv_resultRegion2, out hv_result);

                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                if (hv_result == 1)
                                {
                                    CameraAppendHMessage0 = new HMsgEntry("2:OK", 40, 10, "green", "window", "box", "false", 32, "mono", "true", "false");
                                    CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                                }
                                else
                                {
                                    CameraAppendHMessage0 = new HMsgEntry("2:NG", 40, 10, "red", "window", "box", "false", 32, "mono", "true", "false");
                                    CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                                }
                                CameraAppendHObject0 = hv_resultRegion2;
                            }));
                            if (plcConnect)
                                plc.WriteMCoil(803, !(hv_result == 1));
                        }
                    }
                    
                      
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(100);
            }
        }
        private void LoadCameraParm()
        {
            string filepath = $"Camera\\4";
            try
            {
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "RotateDeg.tup"), out RotateDeg);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin.tup"), out thresholdMin);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax.tup"), out thresholdMax);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width.tup"), out OpeningRec1Width);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height.tup"), out OpeningRec1Height);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax.tup"), out GapMax);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin_2.tup"), out thresholdMin_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax_2.tup"), out thresholdMax_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width_2.tup"), out OpeningRec1Width_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height_2.tup"), out OpeningRec1Height_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax_2.tup"), out GapMax_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "IsExcludeRobotMove.tup"), out IsExcludeRobotMove);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyWidth.tup"), out MussyWidth);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyHeight.tup"), out MussyHeight);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "diffShapeArea.tup"), out diffShapeArea);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "diffShapeHeight.tup"), out diffShapeHeight);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyWidth_2.tup"), out MussyWidth_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyHeight_2.tup"), out MussyHeight_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "diffShapeArea_2.tup"), out diffShapeArea_2);
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "diffShapeHeight_2.tup"), out diffShapeHeight_2);
                HOperatorSet.ReadImage(out iamgeSTX, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, $"executeRec1.jpg"));
                HOperatorSet.ReadRegion(out executeRec1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "executeRec1.hobj"));
                HOperatorSet.ReadRegion(out rec1_0, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_0.hobj"));
                HOperatorSet.ReadRegion(out rec1_1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_1.hobj"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
        #endregion
    }
}
