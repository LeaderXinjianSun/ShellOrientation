

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
        private string messageStr;
        public string MessageStr
        {
            get { return messageStr; }
            set { SetProperty(ref messageStr, value); }
        }
        #endregion
        #region 构造函数
        public CameraViewModel1(IContainerProvider containerProvider, IEventAggregator _aggregator)
        {
            MessageStr = string.Empty;
            aggregator = _aggregator;
            LoadParam();
            cam = containerProvider.Resolve<ICameraService>("Cam1");
            plc = containerProvider.Resolve<IPLCModbusService>("plc");
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
        private void addMessage(string str)
        {
            logger.Debug(str);
            Console.WriteLine(str);
            string[] s = MessageStr.Split('\n');
            if (s.Length > 1000)
            {
                MessageStr = "";
            }
            if (MessageStr != "")
            {
                MessageStr += "\n";
            }
            MessageStr += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + str;
        }
        private void Run(CancellationToken token)
        {
            string filepath = $"Camera\\1";

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    HObject line1, line2, line3;
                    HOperatorSet.ReadRegion(out line1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line0.hobj"));
                    HOperatorSet.ReadRegion(out line2, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line1.hobj"));
                    HOperatorSet.ReadRegion(out line3, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line2.hobj"));
                    CameraIamge0 = cam.GrabeImageAsync();
                    HObject red, green, blue;
                    HOperatorSet.Decompose3(CameraIamge0, out red, out green, out blue);

                    int hv_Result0 = default; HObject ho_ResultRegion0 = default;
                    HOperatorSet.GenEmptyObj(out ho_ResultRegion0);
                    ImageCalc.Calc1(red, line1, ref hv_Result0, ref ho_ResultRegion0);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        CameraAppendHObject0 = null;
                        if (hv_Result0 != 0)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                            CameraAppendHObject0 = ho_ResultRegion0;
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                            CameraAppendHObject0 = ho_ResultRegion0;
                        }
                    }));


                    int hv_Result1 = default; HObject ho_ResultRegion1 = default;
                    HOperatorSet.GenEmptyObj(out ho_ResultRegion1);
                    ImageCalc.Calc1(red, line2, ref hv_Result1, ref ho_ResultRegion1);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_Result1 != 0)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                            CameraAppendHObject0 = ho_ResultRegion1;
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                            CameraAppendHObject0 = ho_ResultRegion1;
                        }
                    }));


                    int hv_Result2 = default; HObject ho_ResultRegion2 = default;
                    HOperatorSet.GenEmptyObj(out ho_ResultRegion2);
                    ImageCalc.Calc1(red, line3, ref hv_Result2, ref ho_ResultRegion2);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_Result2 != 0)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                            CameraAppendHObject0 = ho_ResultRegion2;
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                            CameraAppendHObject0 = ho_ResultRegion2;
                        }
                    }));


                    plc.WriteMCoil(800, !(hv_Result0 != 0 || hv_Result1 != 0 || hv_Result2 != 0));

                    HObject line4, line5, line6;
                    HOperatorSet.ReadRegion(out line4, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line3.hobj"));
                    HOperatorSet.ReadRegion(out line5, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line4.hobj"));
                    HOperatorSet.ReadRegion(out line6, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line5.hobj"));

                    int hv_Result3 = default; HObject ho_ResultRegion3 = default;
                    HOperatorSet.GenEmptyObj(out ho_ResultRegion3);
                    ImageCalc.Calc1(red, line4, ref hv_Result3, ref ho_ResultRegion3);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_Result3 != 0)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                            CameraAppendHObject0 = ho_ResultRegion3;
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                            CameraAppendHObject0 = ho_ResultRegion3;
                        }
                    }));


                    int hv_Result4 = default; HObject ho_ResultRegion4 = default;
                    HOperatorSet.GenEmptyObj(out ho_ResultRegion4);
                    ImageCalc.Calc1(red, line5, ref hv_Result4, ref ho_ResultRegion4);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_Result4 != 0)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                            CameraAppendHObject0 = ho_ResultRegion4;
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                            CameraAppendHObject0 = ho_ResultRegion4;
                        }
                    }));


                    int hv_Result5 = default; HObject ho_ResultRegion5 = default;
                    HOperatorSet.GenEmptyObj(out ho_ResultRegion5);
                    ImageCalc.Calc1(red, line6, ref hv_Result5, ref ho_ResultRegion5);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_Result5 != 0)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                            CameraAppendHObject0 = ho_ResultRegion5;
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                            CameraAppendHObject0 = ho_ResultRegion5;
                        }
                    }));


                    plc.WriteMCoil(801, !(hv_Result3 != 0 || hv_Result4 != 0 || hv_Result5 != 0));
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {

                }

            }
        }
        #endregion
    }
}
