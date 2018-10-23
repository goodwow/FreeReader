using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FreeReader
{
    class OpacityPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int opacity = (int)(CommonConvert.ToDouble(value) * 100);
            return opacity.ToString() + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string optValue = ((string)value).Replace("%", "");
            double opacity = CommonConvert.ToDouble(optValue) / 100.0;
            opacity = opacity <= 0 ? 0.005 : opacity;

            return opacity;
        }
    }
}
