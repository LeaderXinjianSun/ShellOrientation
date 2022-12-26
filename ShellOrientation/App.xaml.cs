using Prism.Ioc;
using ShellOrientation.Common.Services;
using ShellOrientation.ViewModels.Dialog;
using ShellOrientation.ViewModels.Home;
using ShellOrientation.Views;
using ShellOrientation.Views.Dialogs;
using ShellOrientation.Views.Home;
using System;
using System.Threading;
using System.Windows;

namespace ShellOrientation
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        private static Mutex _mutex = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "ShellOrientation";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("ShellOrientation件已开启", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                //app is already running! Exiting the application  
                Environment.Exit(-1);
            }

            base.OnStartup(e);
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CameraView1, CameraViewModel1>("View1");
            containerRegistry.RegisterForNavigation<CameraView2, CameraViewModel2>("View2");
            containerRegistry.RegisterForNavigation<CameraView3, CameraViewModel3>("View3");
            containerRegistry.RegisterForNavigation<CameraView4, CameraViewModel4>("View4");
            containerRegistry.RegisterSingleton<ICameraService, CameraService>("Cam1");
            containerRegistry.RegisterSingleton<ICameraService, CameraService>("Cam2");
            containerRegistry.RegisterSingleton<ICameraService, CameraService>("Cam3");
            containerRegistry.RegisterSingleton<ICameraService, CameraService>("Cam4");
            containerRegistry.RegisterSingleton<IPLCModbusService, PLCModbusService>("plc1");
            containerRegistry.RegisterSingleton<IPLCModbusService, PLCModbusService>("plc2");
            containerRegistry.RegisterDialog<CameraCalcDialog, CameraCalcDialogViewModel>();
        }
    }
}
