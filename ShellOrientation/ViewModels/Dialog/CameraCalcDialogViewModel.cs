

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
        private double rotateDeg;
        public double RotateDeg
        {
            get { return rotateDeg; }
            set { SetProperty(ref rotateDeg, value); }
        }
        private double thresholdMin;
        public double ThresholdMin
        {
            get { return thresholdMin; }
            set { SetProperty(ref thresholdMin, value); }
        }
        private double thresholdMax;
        public double ThresholdMax
        {
            get { return thresholdMax; }
            set { SetProperty(ref thresholdMax, value); }
        }
        private double openingRec1Width;
        public double OpeningRec1Width
        {
            get { return openingRec1Width; }
            set { SetProperty(ref openingRec1Width, value); }
        }
        private double openingRec1Height;
        public double OpeningRec1Height
        {
            get { return openingRec1Height; }
            set { SetProperty(ref openingRec1Height, value); }
        }
        private double gapMax;
        public double GapMax
        {
            get { return gapMax; }
            set { SetProperty(ref gapMax, value); }
        }
        private double thresholdMin_2;
        public double ThresholdMin_2
        {
            get { return thresholdMin_2; }
            set { SetProperty(ref thresholdMin_2, value); }
        }
        private double thresholdMax_2;
        public double ThresholdMax_2
        {
            get { return thresholdMax_2; }
            set { SetProperty(ref thresholdMax_2, value); }
        }
        private double openingRec1Width_2;
        public double OpeningRec1Width_2
        {
            get { return openingRec1Width_2; }
            set { SetProperty(ref openingRec1Width_2, value); }
        }
        private double openingRec1Height_2;
        public double OpeningRec1Height_2
        {
            get { return openingRec1Height_2; }
            set { SetProperty(ref openingRec1Height_2, value); }
        }
        private double gapMax_2;
        public double GapMax_2
        {
            get { return gapMax_2; }
            set { SetProperty(ref gapMax_2, value); }
        }
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
        private DelegateCommand<object> textBoxLostFocusEventCommand;
        public DelegateCommand<object> TextBoxLostFocusEventCommand =>
            textBoxLostFocusEventCommand ?? (textBoxLostFocusEventCommand = new DelegateCommand<object>(ExecuteTextBoxLostFocusEventCommand));

        void ExecuteTextBoxLostFocusEventCommand(object obj)
        {
            string filepath = $"Camera\\{index + 1}";
            switch (obj.ToString())
            {
                case "RotateDeg":
                    HOperatorSet.WriteTuple(new HTuple(RotateDeg),System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "RotateDeg.tup"));
                    break;
                case "ThresholdMin":
                    HOperatorSet.WriteTuple(new HTuple(ThresholdMin), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin.tup"));
                    break;
                case "ThresholdMax":
                    HOperatorSet.WriteTuple(new HTuple(ThresholdMax), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax.tup"));
                    break;
                case "OpeningRec1Width":
                    HOperatorSet.WriteTuple(new HTuple(OpeningRec1Width), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width.tup"));
                    break;
                case "OpeningRec1Height":
                    HOperatorSet.WriteTuple(new HTuple(OpeningRec1Height), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height.tup"));
                    break;
                case "GapMax":
                    HOperatorSet.WriteTuple(new HTuple(GapMax), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax.tup"));
                    break;
                case "ThresholdMin_2":
                    HOperatorSet.WriteTuple(new HTuple(ThresholdMin_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin_2.tup"));
                    break;
                case "ThresholdMax_2":
                    HOperatorSet.WriteTuple(new HTuple(ThresholdMax_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax_2.tup"));
                    break;
                case "OpeningRec1Width_2":
                    HOperatorSet.WriteTuple(new HTuple(OpeningRec1Width_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width_2.tup"));
                    break;
                case "OpeningRec1Height_2":
                    HOperatorSet.WriteTuple(new HTuple(OpeningRec1Height_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height_2.tup"));
                    break;
                case "GapMax_2":
                    HOperatorSet.WriteTuple(new HTuple(GapMax_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax_2.tup"));
                    break;
                default:
                    break;
            }
        }
        void ExecuteCalcCommand()
        {
            string filepath = $"Camera\\{index + 1}";
            HObject rec1_0, rec1_1;
            HOperatorSet.ReadRegion(out rec1_0, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_0.hobj"));
            HOperatorSet.ReadRegion(out rec1_1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_1.hobj"));
            HTuple hv_result;HObject hv_resultRegion1;
            ImageCalc.CalcOpeningRec1(CameraIamge0, rec1_0,ThresholdMin,ThresholdMax,OpeningRec1Width,OpeningRec1Height,GapMax,out hv_resultRegion1,out hv_result);
            Console.WriteLine(hv_result.ToString());
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

            HObject hv_resultRegion2;
            ImageCalc.CalcOpeningRec1(CameraIamge0, rec1_1, ThresholdMin_2, ThresholdMax_2, OpeningRec1Width_2, OpeningRec1Height_2, GapMax_2, out hv_resultRegion2, out hv_result);
            Console.WriteLine(hv_result.ToString());
            if (hv_result == 1)
            {
                CameraGCStyle0 = new Tuple<string, object>("Color", "green");
            }
            else
            {
                CameraGCStyle0 = new Tuple<string, object>("Color", "red");
            }
            CameraAppendHObject0 = hv_resultRegion2;


        }
        void ExecuteCreateLineCommand(object obj)
        {
            string filepath = $"Camera\\{index + 1}";
            ROI roi = Global.CameraImageViewer.DrawROI(ROI.ROI_TYPE_RECTANGLE1);
            var line = roi.getRegion();
            CameraAppendHObject0 = null;
            CameraAppendHObject0 = line;
            DirectoryInfo dir = new DirectoryInfo(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
            if (!dir.Exists)
            {
                Directory.CreateDirectory(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
            }
            HOperatorSet.WriteRegion(line, System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, $"rec1_{obj}.hobj"));
        }
        void ExecuteCameraOperateCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "拍照":
                    //CameraIamge0 = cam.GrabeImageAsync();
                    HObject ho_ImageRotate;
                    HOperatorSet.RotateImage(cam.GrabeImageAsync(), out ho_ImageRotate, RotateDeg, "constant");
                    CameraIamge0 = new HImage(ho_ImageRotate);
                    break;
                case "打开":
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Image Files|*.png;*.bmp;*.jpg;*.tif";
                        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            HObject image;
                            HOperatorSet.ReadImage(out image, openFileDialog.FileName);
                            CameraIamge0 = new HImage(image);
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
                case 2:
                    Title = "相机3";
                    cam = container.Resolve<ICameraService>("Cam3");
                    break;
                case 3:
                    Title = "相机4";
                    cam = container.Resolve<ICameraService>("Cam4");
                    break;
                default:
                    break;
            }
            string filepath = $"Camera\\{index + 1}";
            try
            {
                HTuple v1;
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "RotateDeg.tup"),out v1);
                RotateDeg = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin.tup"), out v1);
                ThresholdMin = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax.tup"), out v1);
                ThresholdMax = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width.tup"), out v1);
                OpeningRec1Width = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height.tup"), out v1);
                OpeningRec1Height = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax.tup"), out v1);
                GapMax = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMin_2.tup"), out v1);
                ThresholdMin_2 = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "ThresholdMax_2.tup"), out v1);
                ThresholdMax_2 = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Width_2.tup"), out v1);
                OpeningRec1Width_2 = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "OpeningRec1Height_2.tup"), out v1);
                OpeningRec1Height_2 = v1.D;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "GapMax_2.tup"), out v1);
                GapMax_2 = v1.D;
            }
            catch { }
        }
        #endregion
        public CameraCalcDialogViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            container = containerProvider;
        }
    }
}
