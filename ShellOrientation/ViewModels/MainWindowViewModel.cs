

using Newtonsoft.Json;
using NLog;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using ShellOrientation.Common.Extensions;
using ShellOrientation.Common.Services;
using ShellOrientation.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShellOrientation.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region 变量
        private readonly IRegionManager regionManager;
        public readonly IEventAggregator aggregator;
        private readonly IDialogService dialogService;
        bool isCameraCalcDialog1Show = false;
        ICameraService cam1, cam2,cam3,cam4;
        IPLCModbusService plc1,plc2;
        Param param;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        #region 属性绑定
        public string Title { get; set; } = "ShellOrientationUI";
        public string Version { get; set; } = "1.0";
        private bool camera1State;
        public bool Camera1State
        {
            get { return camera1State; }
            set { SetProperty(ref camera1State, value); }
        }
        private bool camera2State;
        public bool Camera2State
        {
            get { return camera2State; }
            set { SetProperty(ref camera2State, value); }
        }
        private bool camera3State;
        public bool Camera3State
        {
            get { return camera3State; }
            set { SetProperty(ref camera3State, value); }
        }
        private bool camera4State;
        public bool Camera4State
        {
            get { return camera4State; }
            set { SetProperty(ref camera4State, value); }
        }
        private bool pLC1State;
        public bool PLC1State
        {
            get { return pLC1State; }
            set { SetProperty(ref pLC1State, value); }
        }
        private bool pLC2State;
        public bool PLC2State
        {
            get { return pLC2State; }
            set { SetProperty(ref pLC2State, value); }
        }
        #endregion
        #region 方法绑定
        private DelegateCommand appLoadedEventCommand;
        public DelegateCommand AppLoadedEventCommand =>
            appLoadedEventCommand ?? (appLoadedEventCommand = new DelegateCommand(ExecuteAppLoadedEventCommand));
        private DelegateCommand testCommand;
        public DelegateCommand TestCommand =>
            testCommand ?? (testCommand = new DelegateCommand(ExecuteTestCommand));
        private DelegateCommand<object> menuCommand;
        public DelegateCommand<object> MenuCommand =>
            menuCommand ?? (menuCommand = new DelegateCommand<object>(ExecuteMenuCommand));
        private DelegateCommand appClosedEventCommand;
        public DelegateCommand AppClosedEventCommand =>
            appClosedEventCommand ?? (appClosedEventCommand = new DelegateCommand(ExecuteAppClosedEventCommand));
        private DelegateCommand saveImageCommand;
        public DelegateCommand SaveImageCommand =>
            saveImageCommand ?? (saveImageCommand = new DelegateCommand(ExecuteSaveImageCommand));

        void ExecuteSaveImageCommand()
        {            
            aggregator.SendMessage("SaveImage", "App");
        }
        void ExecuteAppClosedEventCommand()
        {
            aggregator.SendMessage("Closed", "App");
            //await Task.Delay(500);
            cam1.CloseCamera();
            cam2.CloseCamera();
            cam3.CloseCamera();
            cam4.CloseCamera();
            try
            {
                plc1.WriteMCoil(800, false);
                plc1.WriteMCoil(801, false);
                plc1.WriteMCoil(802, false);
                plc1.WriteMCoil(803, false);

                plc2.WriteMCoil(800, false);
                plc2.WriteMCoil(801, false);
                plc2.WriteMCoil(802, false);
                plc2.WriteMCoil(803, false);
            }
            catch { }

            plc1.Close();
            plc2.Close();
            logger.Info("软件关闭");
        }
        void ExecuteMenuCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "camera1":
                    if (!isCameraCalcDialog1Show)
                    {
                        isCameraCalcDialog1Show = true;
                        DialogParameters param = new DialogParameters();
                        param.Add("CameraIndex", 0);
                        dialogService.Show("CameraCalcDialog", param, arg =>
                        {
                            isCameraCalcDialog1Show = false;
                            aggregator.SendMessage("ReloadCamera1Param", "App");
                        });
                    }
                    break;
                case "camera2":
                    if (!isCameraCalcDialog1Show)
                    {
                        isCameraCalcDialog1Show = true;
                        DialogParameters param = new DialogParameters();
                        param.Add("CameraIndex", 1);
                        dialogService.Show("CameraCalcDialog", param, arg =>
                        {
                            isCameraCalcDialog1Show = false;
                            aggregator.SendMessage("ReloadCamera2Param", "App");
                        });
                    }
                    break;
                case "camera3":
                    if (!isCameraCalcDialog1Show)
                    {
                        isCameraCalcDialog1Show = true;
                        DialogParameters param = new DialogParameters();
                        param.Add("CameraIndex", 2);
                        dialogService.Show("CameraCalcDialog", param, arg =>
                        {
                            isCameraCalcDialog1Show = false;
                            aggregator.SendMessage("ReloadCamera3Param", "App");
                        });
                    }
                    break;
                case "camera4":
                    if (!isCameraCalcDialog1Show)
                    {
                        isCameraCalcDialog1Show = true;
                        DialogParameters param = new DialogParameters();
                        param.Add("CameraIndex", 3);
                        dialogService.Show("CameraCalcDialog", param, arg =>
                        {
                            isCameraCalcDialog1Show = false;
                            aggregator.SendMessage("ReloadCamera4Param", "App");
                        });
                    }
                    break;
                default:
                    break;
            }
        }
        void ExecuteTestCommand()
        {
            //var param = new Param() {
            //    Camera1Name = "1",
            //    Camera2Name = "2",
            //    PLCIP = "192.168.0.1",
            //};
            //string jsonString = JsonConvert.SerializeObject(param, Formatting.Indented);
            //File.WriteAllText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"), jsonString);
            //Console.WriteLine("Success");
        }
        async void ExecuteAppLoadedEventCommand()
        {
            logger.Info("软件开启");
            regionManager.Regions["CameraRegion1"].RequestNavigate("View1");
            regionManager.Regions["CameraRegion2"].RequestNavigate("View2");
            regionManager.Regions["CameraRegion3"].RequestNavigate("View3");
            regionManager.Regions["CameraRegion4"].RequestNavigate("View4");
            var r1 = await Task.Run(() => { return plc1.Connect(param.PLCIP1); });
            PLC1State = r1;
            if (r1)
            {
                aggregator.SendMessage("PLC1Connect", "App");
            }
            else
            {
                MessageBox.Show($"PLC:{param.PLCIP1}连接失败", "确认", MessageBoxButtons.OK);
            }
            var r2 = await Task.Run(() => { return plc2.Connect(param.PLCIP2); });
            PLC2State = r2;
            if (r2)
            {
                aggregator.SendMessage("PLC2Connect", "App");
            }
            else
            {
                MessageBox.Show($"PLC:{param.PLCIP2}连接失败", "确认", MessageBoxButtons.OK);
            }
            
        }
        #endregion
        #region 构造函数
        public MainWindowViewModel(IContainerProvider containerProvider, IRegionManager _regionManager, IEventAggregator _aggregator, IDialogService _dialogService)
        {
            Camera1State = false;
            Camera2State = false;
            Camera3State = false;
            Camera4State = false;
            PLC1State = false;
            PLC2State = false;
            regionManager = _regionManager;
            aggregator = _aggregator;
            dialogService = _dialogService;
            cam1 = containerProvider.Resolve<ICameraService>("Cam1");
            cam2 = containerProvider.Resolve<ICameraService>("Cam2");
            cam3 = containerProvider.Resolve<ICameraService>("Cam3");
            cam4 = containerProvider.Resolve<ICameraService>("Cam4");
            plc1 = containerProvider.Resolve<IPLCModbusService>("plc1");
            plc2 = containerProvider.Resolve<IPLCModbusService>("plc2");
            LoadParam();

            NlogConfig();
            aggregator.ResgiterMessage(arg => {
                switch (arg.Message)
                {
                    case "Camera1OpenOK":
                        Camera1State = true;
                        break;
                    case "Camera1OpenNG":
                        Camera1State = false;
                        break;
                    case "Camera2OpenOK":
                        Camera2State = true;
                        break;
                    case "Camera2OpenNG":
                        Camera2State = false;
                        break;
                    case "Camera3OpenOK":
                        Camera3State = true;
                        break;
                    case "Camera3OpenNG":
                        Camera3State = false;
                        break;
                    case "Camera4OpenOK":
                        Camera4State = true;
                        break;
                    case "Camera4OpenNG":
                        Camera4State = false;
                        break;
                    default:
                        break;
                }
            }, "Camera");
        }
        #endregion
        #region 功能函数
        private void NlogConfig()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "${basedir}/logs/${shortdate}.log", Layout = "${longdate}|${level:uppercase=true}|${message}" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }
        private void LoadParam()
        {
            //Json序列化，从文件读取
            string jsonString = File.ReadAllText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Param.json"));
            param = JsonConvert.DeserializeObject<Param>(jsonString);
        }
        #endregion
    }
}
