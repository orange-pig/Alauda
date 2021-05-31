using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alauda
{
    public class LongBytesToFormatStringConverter : IValueConverter
    {
        private bool isUseSpeed;

        public bool IsUseSpeed
        {
            get
            {
                return isUseSpeed;
            }

            set
            {
                isUseSpeed = value;
            }
        }

        public LongBytesToFormatStringConverter()
            : this(false)
        {
        }

        public LongBytesToFormatStringConverter(bool isUseSpeed)
        {
            this.IsUseSpeed = isUseSpeed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long bytes;
            if (!long.TryParse(value.ToString(), out bytes))
            {
                return string.Empty;
            }

            if (isUseSpeed)
            {
                return FileSizeToSpeedDisplayString(bytes);
            }
            else
            {
                return FileSizeToDisplayString(bytes);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }


        /// <summary>
        /// 将字节大小转换为显示字符串
        /// </summary>
        /// <param name="size"></param>
        private string FileSizeToSpeedDisplayString(long size)
        {
            var displayValue = 0d;
            var devimalDigit = 0;
            var unit = "B/s";

            if (size < 1024L)
            {
                displayValue = size;
                devimalDigit = 0;
            }
            else if (size < Math.Pow(1024L, 2))
            {
                displayValue = size / 1024;
                devimalDigit = 0;
                unit = "KB/s";
            }
            else if (size < Math.Pow(1024L, 3))
            {
                displayValue = size / Math.Pow(1024L, 2);
                devimalDigit = 2;
                unit = "MB/s";
            }
            else if (size < Math.Pow(1024L, 4))
            {
                displayValue = size / Math.Pow(1024L, 3);
                devimalDigit = 4;
                unit = "GB/s";
            }
            else if (size < Math.Pow(1024L, 5))
            {
                displayValue = size / Math.Pow(1024L, 4);
                devimalDigit = 6;
                unit = "TB/s";
            }
            else if (size < Math.Pow(1024L, 6))
            {
                displayValue = size / Math.Pow(1024L, 5);
                devimalDigit = 6;
                unit = "PB/s";
            }
            else
            {
                displayValue = size;
                devimalDigit = 0;
            }

            return string.Format("{0:F" + devimalDigit + "} {1}", displayValue, unit);
        }


        /// <summary>
        /// 将字节大小转换为显示字符串
        /// </summary>
        /// <param name="size"></param>
        private string FileSizeToDisplayString(long size)
        {
            var displayValue = 0d;
            var devimalDigit = 0;
            var unit = "B";

            if (size < 1024L)
            {
                displayValue = size;
                devimalDigit = 0;
            }
            else if (size < Math.Pow(1024L, 2))
            {
                displayValue = size / 1024;
                devimalDigit = 0;
                unit = "KB";
            }
            else if (size < Math.Pow(1024L, 3))
            {
                displayValue = size / Math.Pow(1024L, 2);
                devimalDigit = 2;
                unit = "MB";
            }
            else if (size < Math.Pow(1024L, 4))
            {
                displayValue = size / Math.Pow(1024L, 3);
                devimalDigit = 4;
                unit = "GB";
            }
            else if (size < Math.Pow(1024L, 5))
            {
                displayValue = size / Math.Pow(1024L, 4);
                devimalDigit = 6;
                unit = "TB";
            }
            else if (size < Math.Pow(1024L, 6))
            {
                displayValue = size / Math.Pow(1024L, 5);
                devimalDigit = 6;
                unit = "PB";
            }
            else
            {
                displayValue = size;
                devimalDigit = 0;
            }

            return string.Format("{0:F" + devimalDigit + "} {1}", displayValue, unit);
        }
    }
}
