
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
    public class CameraViewModel2 : BindableBase
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
        public CameraViewModel2(IContainerProvider containerProvider, IEventAggregator _aggregator)
        {
            MessageStr = string.Empty;
            aggregator = _aggregator;
            LoadParam();
            cam = containerProvider.Resolve<ICameraService>("Cam2");
            plc = containerProvider.Resolve<IPLCModbusService>("plc");
            var r = cam.OpenCamera(param.Camera2Name, "DirectShow");//[0] Integrated Camera //[1] LRCP  USB2.0
            //M800-M803
            if (r)
            {
                cam.GrabeImageStart();
                aggregator.SendMessage("Camera2OpenOK", "Camera");
                source = new CancellationTokenSource();
                CancellationToken token1 = source.Token;
                Task.Run(() => Run(token1), token1);
            }
            else
            {
                aggregator.SendMessage("Camera2OpenNG", "Camera");
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
            string filepath = $"Camera\\2";
            HObject rec2_0, rec2_1;
            HOperatorSet.ReadRegion(out rec2_0, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec2_0.hobj"));
            HOperatorSet.ReadRegion(out rec2_1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec2_1.hobj"));
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                try
                {

                    CameraIamge0 = cam.GrabeImageAsync();
                    HTuple hv_result; HObject hv_resultRegion1;
                    ImageCalc.Calc1(CameraIamge0, rec2_0, out hv_resultRegion1, 50, 150, 300, 10, 20, out hv_result);
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_result.I == 1)
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
                    plc.WriteMCoil(802, !(hv_result.I == 1));

                    HTuple hv_result2; HObject hv_resultRegion2;
                    ImageCalc.Calc1(CameraIamge0, rec2_1, out hv_resultRegion2, 50, 150, 300, 10, 20, out hv_result2);
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (hv_result2.I == 1)
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "green");
                        }
                        else
                        {
                            CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                        }
                        CameraAppendHObject0 = hv_resultRegion2;
                    }));
                    plc.WriteMCoil(803, !(hv_result2.I == 1));


                }
                catch (Exception ex)
                {

                }

            }
        }
        #endregion
    }
}
