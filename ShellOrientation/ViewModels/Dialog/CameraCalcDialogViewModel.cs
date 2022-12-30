

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
        private bool isExcludeRobotMove;
        public bool IsExcludeRobotMove
        {
            get { return isExcludeRobotMove; }
            set { SetProperty(ref isExcludeRobotMove, value); }
        }
        private double mussyWidth;
        public double MussyWidth
        {
            get { return mussyWidth; }
            set { SetProperty(ref mussyWidth, value); }
        }
        private double mussyHeight;
        public double MussyHeight
        {
            get { return mussyHeight; }
            set { SetProperty(ref mussyHeight, value); }
        }
        private int diffShapeArea;
        public int DiffShapeArea
        {
            get { return diffShapeArea; }
            set { SetProperty(ref diffShapeArea, value); }
        }
        private int diffShapeHeight;
        public int DiffShapeHeight
        {
            get { return diffShapeHeight; }
            set { SetProperty(ref diffShapeHeight, value); }
        }
        private double mussyWidth_2;
        public double MussyWidth_2
        {
            get { return mussyWidth_2; }
            set { SetProperty(ref mussyWidth_2, value); }
        }
        private double mussyHeight_2;
        public double MussyHeight_2
        {
            get { return mussyHeight_2; }
            set { SetProperty(ref mussyHeight_2, value); }
        }
        private int diffShapeArea_2;
        public int DiffShapeArea_2
        {
            get { return diffShapeArea_2; }
            set { SetProperty(ref diffShapeArea_2, value); }
        }
        private int diffShapeHeight_2;
        public int DiffShapeHeight_2
        {
            get { return diffShapeHeight_2; }
            set { SetProperty(ref diffShapeHeight_2, value); }
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
        private DelegateCommand checkBoxCommand;
        public DelegateCommand CheckBoxCommand =>
            checkBoxCommand ?? (checkBoxCommand = new DelegateCommand(ExecuteCheckBoxCommand));
        private DelegateCommand createExcludeRobotMoveCommand;
        public DelegateCommand CreateExcludeRobotMoveCommand =>
            createExcludeRobotMoveCommand ?? (createExcludeRobotMoveCommand = new DelegateCommand(ExecuteCreateExcludeRobotMoveCommand));

        void ExecuteCreateExcludeRobotMoveCommand()
        {
            string filepath = $"Camera\\{index + 1}";
            try
            {
                ROI roi = Global.CameraImageViewer.DrawROI(ROI.ROI_TYPE_RECTANGLE1);
                var executeRec1 = roi.getRegion();
                CameraGCStyle0 = new Tuple<string, object>("Color", "cyan");
                CameraAppendHObject0 = null;
                CameraAppendHObject0 = executeRec1;
                HTuple executeRec1Row1;
                HOperatorSet.RegionFeatures(executeRec1, "row1", out executeRec1Row1);
                HTuple executeRec1Column1;
                HOperatorSet.RegionFeatures(executeRec1, "column1", out executeRec1Column1);
                HTuple executeRec1Width;
                HOperatorSet.RegionFeatures(executeRec1, "width", out executeRec1Width);
                HTuple executeRec1Height;
                HOperatorSet.RegionFeatures(executeRec1, "height", out executeRec1Height);
                HObject imagePart;
                HOperatorSet.CropPart(CameraIamge0, out imagePart, executeRec1Row1, executeRec1Column1, executeRec1Width, executeRec1Height);
                DirectoryInfo dir = new DirectoryInfo(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
                if (!dir.Exists)
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath));
                }
                HOperatorSet.WriteRegion(executeRec1, System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "executeRec1.hobj"));
                HOperatorSet.WriteImage(imagePart, "jpeg", 0, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "executeRec1.jpg"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        void ExecuteCheckBoxCommand()
        {
            string filepath = $"Camera\\{index + 1}";
            try
            {
                int v1 = IsExcludeRobotMove ? 1 : 0;
                HOperatorSet.WriteTuple(new HTuple(v1), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "IsExcludeRobotMove.tup"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        void ExecuteTextBoxLostFocusEventCommand(object obj)
        {
            string filepath = $"Camera\\{index + 1}";
            try
            {
                switch (obj.ToString())
                {
                    case "RotateDeg":
                        HOperatorSet.WriteTuple(new HTuple(RotateDeg), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "RotateDeg.tup"));
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
                    case "MussyWidth":
                        HOperatorSet.WriteTuple(new HTuple(MussyWidth), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyWidth.tup"));
                        break;
                    case "MussyHeight":
                        HOperatorSet.WriteTuple(new HTuple(MussyHeight), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyHeight.tup"));
                        break;
                    case "DiffShapeArea":
                        HOperatorSet.WriteTuple(new HTuple(DiffShapeArea), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeArea.tup"));
                        break;
                    case "DiffShapeHeight":
                        HOperatorSet.WriteTuple(new HTuple(DiffShapeHeight), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeHeight.tup"));
                        break;
                    case "MussyWidth_2":
                        HOperatorSet.WriteTuple(new HTuple(MussyWidth_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyWidth_2.tup"));
                        break;
                    case "MussyHeight_2":
                        HOperatorSet.WriteTuple(new HTuple(MussyHeight_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyHeight_2.tup"));
                        break;
                    case "DiffShapeArea_2":
                        HOperatorSet.WriteTuple(new HTuple(DiffShapeArea_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeArea_2.tup"));
                        break;
                    case "DiffShapeHeight_2":
                        HOperatorSet.WriteTuple(new HTuple(DiffShapeHeight_2), System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeHeight_2.tup"));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        void ExecuteCalcCommand()
        {
            string filepath = $"Camera\\{index + 1}";
            try
            {
                if (IsExcludeRobotMove)
                {
                    HObject iamgeSTX;
                    HOperatorSet.ReadImage(out iamgeSTX, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, $"executeRec1.jpg"));
                    HObject executeRec1;
                    HOperatorSet.ReadRegion(out executeRec1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "executeRec1.hobj"));
                    HObject ho_resultRegion;
                    HTuple hv_result0;
                    ImageCalc.SubImage(CameraIamge0, iamgeSTX, executeRec1, out ho_resultRegion, out hv_result0);

                    if (hv_result0.I == 0)
                    {
                        CameraGCStyle0 = new Tuple<string, object>("DrawMode", "fill");
                        CameraGCStyle0 = new Tuple<string, object>("Color", "orange red");
                        CameraAppendHObject0 = null;
                        CameraAppendHObject0 = ho_resultRegion;
                        CameraAppendHMessage0 = null;
                        CameraAppendHMessage0 = new HMsgEntry("画面有干扰，不做计算。", 10, 10, "red", "window", "box", "false", 32, "mono", "true", "false");
                        return;
                    }
                    iamgeSTX.Dispose();
                    executeRec1.Dispose();
                }
                HObject rec1_0, rec1_1;
                HOperatorSet.ReadRegion(out rec1_0, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_0.hobj"));
                HOperatorSet.ReadRegion(out rec1_1, System.IO.Path.Combine(System.Environment.CurrentDirectory, filepath, "rec1_1.hobj"));
                HTuple hv_result; HObject hv_resultRegion1;
                ImageCalc.CalcOpeningRec1(CameraIamge0, rec1_0, ThresholdMin, ThresholdMax, OpeningRec1Width, OpeningRec1Height, GapMax, MussyWidth, MussyHeight, DiffShapeArea, DiffShapeHeight, out hv_resultRegion1, out hv_result);

                CameraAppendHMessage0 = null;
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
                CameraAppendHObject0 = null;
                CameraAppendHObject0 = hv_resultRegion1;

                HObject hv_resultRegion2;
                ImageCalc.CalcOpeningRec1(CameraIamge0, rec1_1, ThresholdMin_2, ThresholdMax_2, OpeningRec1Width_2, OpeningRec1Height_2, GapMax_2, MussyWidth_2, MussyHeight_2, DiffShapeArea_2, DiffShapeHeight_2, out hv_resultRegion2, out hv_result);

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
            }
            catch (Exception ex)
            {
                CameraAppendHMessage0 = new HMsgEntry("计算出错。", 40, 10, "red", "window", "box", "false", 32, "mono", "true", "false");
                CameraGCStyle0 = new Tuple<string, object>("Color", "red");
                Console.WriteLine(ex.Message);
            }
        }
        void ExecuteCreateLineCommand(object obj)
        {
            string filepath = $"Camera\\{index + 1}";
            ROI roi = Global.CameraImageViewer.DrawROI(ROI.ROI_TYPE_RECTANGLE1);
            var line = roi.getRegion();
            CameraGCStyle0 = new Tuple<string, object>("Color", "violet");
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
                    try
                    {
                        HObject ho_ImageRotate;
                        var img = cam.GrabeImageAsync();
                        if (img != null)
                        {
                            HOperatorSet.RotateImage(img, out ho_ImageRotate, RotateDeg, "constant");
                            CameraIamge0 = new HImage(ho_ImageRotate);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
               
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

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "IsExcludeRobotMove.tup"), out v1);
                IsExcludeRobotMove = v1.I == 1;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyWidth.tup"), out v1);
                MussyWidth = v1.D;
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyHeight.tup"), out v1);
                MussyHeight = v1.D;
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeArea.tup"), out v1);
                DiffShapeArea = v1.I;
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeHeight.tup"), out v1);
                DiffShapeHeight = v1.I;

                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyWidth_2.tup"), out v1);
                MussyWidth_2 = v1.D;
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "MussyHeight_2.tup"), out v1);
                MussyHeight_2 = v1.D;
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeArea_2.tup"), out v1);
                DiffShapeArea_2 = v1.I;
                HOperatorSet.ReadTuple(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filepath, "DiffShapeHeight_2.tup"), out v1);
                DiffShapeHeight_2 = v1.I;
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
