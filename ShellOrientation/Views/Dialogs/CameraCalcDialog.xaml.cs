
using System.Windows.Controls;

namespace ShellOrientation.Views.Dialogs
{
    /// <summary>
    /// CameraCalcDialog.xaml 的交互逻辑
    /// </summary>
    public partial class CameraCalcDialog : UserControl
    {
        public CameraCalcDialog()
        {
            InitializeComponent();
            Global.CameraImageViewer = CameraImageViewer;
        }
    }
}
