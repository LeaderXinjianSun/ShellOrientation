

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ShellOrientation.Common.Converters
{
    public class Boolean2ColorGrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v1)
            {
                return v1 ? new SolidColorBrush(Colors.Lime) : new SolidColorBrush(Colors.Gray);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default(bool);
        }
    }
}
