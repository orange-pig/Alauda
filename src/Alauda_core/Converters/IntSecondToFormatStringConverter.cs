using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alauda
{
    public class IntSecondToFormatStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int timeSecond;
            if (!int.TryParse(value.ToString(), out timeSecond))
            {
                return string.Empty;
            }

            var timeSpan = TimeSpan.FromSeconds(timeSecond);
            var format = @"mm\:ss";
            if (timeSpan.Hours > 0)
            {
                format = @"hh\:mm\:ss";
            }
            return timeSpan.ToString(format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeString = value.ToString();
            TimeSpan span;
            if (TimeSpan.TryParse(timeString, out span))
            {
                return (int)span.TotalSeconds;
            }

            return 0;
        }
    }
}
