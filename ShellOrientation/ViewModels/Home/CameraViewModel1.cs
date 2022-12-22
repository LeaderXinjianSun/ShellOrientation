

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
                Task.Run(()=> {
                    while (true)
                    {
                        try
                        {
                            CameraIamge0 = cam.GrabeImageAsync();
                        }
                        catch (Exception ex)
                        {

                        }
                        
                    }
                    
                });
            }
            else
            {
                aggregator.SendMessage("Camera1OpenNG", "Camera");
            }
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

        #endregion
    }
}
