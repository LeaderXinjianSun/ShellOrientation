

using Newtonsoft.Json;
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

namespace ShellOrientation.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region 变量
        private readonly IRegionManager regionManager;
        public readonly IEventAggregator aggregator;
        private readonly IDialogService dialogService;
        bool isCameraCalcDialog1Show = false;
        ICameraService cam1, cam2;
        IPLCModbusService plc;
        Param param;
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
        private bool pLCState;
        public bool PLCState
        {
            get { return pLCState; }
            set { SetProperty(ref pLCState, value); }
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

        async void ExecuteAppClosedEventCommand()
        {
            aggregator.SendMessage("Closed", "App");
            await Task.Delay(500);
            cam1.CloseCamera();
            cam2.CloseCamera();
            try
            {
                plc.WriteMCoil(800, false);
                plc.WriteMCoil(801, false);
                plc.WriteMCoil(802, false);
                plc.WriteMCoil(803, false);
            }
            catch { }

            plc.Close();
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
        void ExecuteAppLoadedEventCommand()
        {
            regionManager.Regions["CameraRegion1"].RequestNavigate("View1");
            regionManager.Regions["CameraRegion2"].RequestNavigate("View2");
        }
        #endregion
        #region 构造函数
        public MainWindowViewModel(IContainerProvider containerProvider, IRegionManager _regionManager, IEventAggregator _aggregator, IDialogService _dialogService)
        {
            Camera1State = false;
            Camera2State = false;
            PLCState = false;
            regionManager = _regionManager;
            aggregator = _aggregator;
            dialogService = _dialogService;
            cam1 = containerProvider.Resolve<ICameraService>("Cam1");
            cam2 = containerProvider.Resolve<ICameraService>("Cam2");
            plc = containerProvider.Resolve<IPLCModbusService>("plc");
            LoadParam();
            var r1 = plc.Connect(param.PLCIP);
            PLCState = r1;
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
