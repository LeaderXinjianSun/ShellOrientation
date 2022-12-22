
using System.Windows.Controls;

namespace ShellOrientation.Views.Home
{
    /// <summary>
    /// CameraView.xaml 的交互逻辑
    /// </summary>
    public partial class CameraView1 : UserControl
    {
        public CameraView1()
        {
            InitializeComponent();
        }

        private void MsgTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MsgTextBox.ScrollToEnd();
        }
    }
}
