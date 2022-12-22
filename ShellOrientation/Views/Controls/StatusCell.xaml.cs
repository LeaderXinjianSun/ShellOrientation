using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

namespace ShellOrientation.Views.Controls
{
    /// <summary>
    /// StatusCell.xaml 的交互逻辑
    /// </summary>
    public partial class StatusCell : UserControl
    {
        public StatusCell()
        {
            InitializeComponent();
        }
        public string StateName
        {
            get { return (string)GetValue(StateNameProperty); }
            set { SetValue(StateNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StateName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateNameProperty =
            DependencyProperty.Register("StateName", typeof(string), typeof(StatusCell));



        public bool State
        {
            get { return (bool)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(bool), typeof(StatusCell));
    }
}
