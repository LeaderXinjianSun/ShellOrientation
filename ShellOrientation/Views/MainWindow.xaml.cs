
using System.ComponentModel;
using System.Windows;

namespace ShellOrientation.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {

            if (MessageBox.Show("你确定关闭软件吗？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {

            }
            else
                e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
