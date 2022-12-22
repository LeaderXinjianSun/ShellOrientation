

using HalconDotNet;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using ShellOrientation.Common.Services;
using ShellOrientation.Models;
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
            int hv_Result0 = default; HObject ho_ResultRegion0 = default;
            HOperatorSet.GenEmptyObj(out ho_ResultRegion0);
            ImageCalc.Calc1(CameraIamge0, line1,ref hv_Result0, ref ho_ResultRegion0);
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

            int hv_Result1 = default; HObject ho_ResultRegion1 = default;
            HOperatorSet.GenEmptyObj(out ho_ResultRegion1);
            ImageCalc.Calc1(CameraIamge0, line2, ref hv_Result1, ref ho_ResultRegion1);

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

            int hv_Result2 = default; HObject ho_ResultRegion2 = default;
            HOperatorSet.GenEmptyObj(out ho_ResultRegion2);
            ImageCalc.Calc1(CameraIamge0, line3, ref hv_Result2, ref ho_ResultRegion2);

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

            HObject line4, line5, line6;
            HOperatorSet.ReadRegion(out line4, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line3.hobj"));
            HOperatorSet.ReadRegion(out line5, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line4.hobj"));
            HOperatorSet.ReadRegion(out line6, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "line5.hobj"));
            int hv_Result3 = default; HObject ho_ResultRegion3 = default;
            HOperatorSet.GenEmptyObj(out ho_ResultRegion3);
            ImageCalc.Calc1(CameraIamge0, line4, ref hv_Result3, ref ho_ResultRegion3);

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

            int hv_Result4 = default; HObject ho_ResultRegion4 = default;
            HOperatorSet.GenEmptyObj(out ho_ResultRegion4);
            ImageCalc.Calc1(CameraIamge0, line5, ref hv_Result4, ref ho_ResultRegion4);

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

            int hv_Result5 = default; HObject ho_ResultRegion5 = default;
            HOperatorSet.GenEmptyObj(out ho_ResultRegion5);
            ImageCalc.Calc1(CameraIamge0, line6, ref hv_Result5, ref ho_ResultRegion5);

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
