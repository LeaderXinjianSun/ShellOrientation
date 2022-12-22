﻿

using HalconDotNet;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using ShellOrientation.Common.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using ViewROI;

namespace ShellOrientation.ViewModels.Dialog
{
    public class CameraCalcDialogViewModel : DialogViewModel
    {
        #region 变量
        int index = 0;
        private readonly IContainerProvider container;
        ICameraService cam;
        #endregion
        #region 属性绑定
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
        #region 方法绑定
        private DelegateCommand<object> cameraOperateCommand;
        public DelegateCommand<object> CameraOperateCommand =>
            cameraOperateCommand ?? (cameraOperateCommand = new DelegateCommand<object>(ExecuteCameraOperateCommand));
        private DelegateCommand<object> createLineCommand;
        public DelegateCommand<object> CreateLineCommand =>
            createLineCommand ?? (createLineCommand = new DelegateCommand<object>(ExecuteCreateLineCommand));
        private DelegateCommand calcCommand;
        public DelegateCommand CalcCommand =>
            calcCommand ?? (calcCommand = new DelegateCommand(ExecuteCalcCommand));

        void ExecuteCalcCommand()
        {
            string filepath = $"Camera\\{index + 1}";
            HObject line1, line2, line3;
            HOperatorSet.ReadRegion(out line1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line0.hobj"));
            HOperatorSet.ReadRegion(out line2, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line1.hobj"));
            HOperatorSet.ReadRegion(out line3, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line2.hobj"));
            CameraGCStyle0 = new Tuple<string, object>("Color", "yellow");
            CameraAppendHObject0 = null;
            CameraAppendHObject0 = line1;
            CameraAppendHObject0 = line2;
            CameraAppendHObject0 = line3;
        }
        void ExecuteCreateLineCommand(object obj)
        {
            string filepath = $"Camera\\{index + 1}";
            ROI roi = Global.CameraImageViewer.DrawROI(ROI.ROI_TYPE_LINE);
            var line = roi.getRegion();
            CameraAppendHObject0 = null;
            CameraAppendHObject0 = line;
            DirectoryInfo dir = new DirectoryInfo(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
            if (!dir.Exists)
            {
                Directory.CreateDirectory(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
            }
            HOperatorSet.WriteRegion(line, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, $"line{obj}.hobj"));
        }
        void ExecuteCameraOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "拍照":
                    CameraIamge0 = cam.GrabeImageAsync();
                    break;
                case "打开":
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Image Files|*.png;*.bmp;*.jpg;*.tif";
                        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            HObject image;
                            HOperatorSet.ReadImage(out image, openFileDialog.FileName);
                            HTuple channels;
                            HOperatorSet.CountChannels(image,out channels);
                            if (channels.I  == 3)
                            {
                                HObject red, green, blue;
                                HOperatorSet.Decompose3(image, out red, out green, out blue);
                                HObject Hue, Saturation, Intensity;
                                HOperatorSet.TransFromRgb(red, green, blue, out Hue,out Saturation,out Intensity,"hsv");
                                CameraIamge0 = new HImage(red);

                            }
                            else
                            {
                                CameraIamge0 = new HImage(image);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 导航
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            index = parameters.GetValue<int>("CameraIndex");
            switch (index)
            {
                case 0:
                    Title = "相机1";
                    cam = container.Resolve<ICameraService>("Cam1");
                    break;
                case 1:
                    Title = "相机2";
                    cam = container.Resolve<ICameraService>("Cam2");
                    break;
                default:
                    break;
            }
           
        }
        #endregion
        public CameraCalcDialogViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            container = containerProvider;
        }
    }
}