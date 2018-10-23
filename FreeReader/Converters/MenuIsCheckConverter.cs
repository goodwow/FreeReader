using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace FreeReader
{
    public class MenuIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CommonConvert.ToDouble(value) == CommonConvert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CommonConvert.ToDouble(parameter);
        }
    }
}