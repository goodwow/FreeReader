using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using FreeReader.Utils;

namespace FreeReader
{
    /// <summary>
    /// Title:  布尔可见转换类
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = CommonConvert.ToBoolean(value);
            if (CommonConvert.ToBoolean(parameter, true))
            {
                return result ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return result ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
