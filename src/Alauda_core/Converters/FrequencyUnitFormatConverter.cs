using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alauda
{
    public class FrequencyUnitFormatConverter : IValueConverter
    {
        private Dictionary<string, ulong> orders = new Dictionary<string, ulong>() {
            { "KHz", 1000 },
            { "MHz", 1000000 },
            { "Hz", 1000000000 }
        };

        public FrequencyUnitFormatConverter()
        {
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return FormatUnitNumber(0);

            double number;
            if (!double.TryParse(value.ToString(), out number))
            {
                FormatUnitNumber(0);
            }

            return FormatUnitNumber(number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UnFormatUnitNumber(value.ToString());
        }


        private string FormatUnitNumber(double value)
        {
            var displayValue = 0d;
            var devimalDigit = 0;
            var unit = "Hz";

            if (value < 1e3 && value > -1e3)
            {
                displayValue = value;
                devimalDigit = 0;
            }
            else if (value < 1e6 && value > -1e6)
            {
                displayValue = value / 1e3;
                devimalDigit = 3;
                unit = "KHz";
            }
            else if (value < 1e18 && value > -1e18)
            {
                displayValue = value / 1e6;
                devimalDigit = 6;
                unit = "MHz";
            }
            //else if (value < 1e12 && value > -1e12)
            //{
            //    displayValue = value / 1e9;
            //    devimalDigit = 6;
            //    unit = "G" + Unit;
            //}
            //else if (value < 1e15 && value > -1e15)
            //{
            //    displayValue = value / 1e12;
            //    devimalDigit = 6;
            //    unit = "T" + Unit;
            //}
            //else if (value < 1e18 && value > -1e18)
            //{
            //    displayValue = value / 1e15;
            //    devimalDigit = 6;
            //    unit = "P" + Unit;
            //}
            else
            {
                displayValue = 1e18;// 用旧值重新覆盖
            }

            return string.Format("{0:F" + devimalDigit + "} {1}", displayValue, unit);
        }

        private double UnFormatUnitNumber(string unitStaring)
        {
            // 为空时
            var text = unitStaring.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            // 为整数时
            double number;
            if (double.TryParse(text, out number))
            {
                return number;
            }

            // 有后缀时
            foreach (var item in orders.Keys)
            {
                if (text.ToLower().EndsWith(item.ToLower()))
                {
                    double numberD;
                    var numText = text.Substring(0, text.Length - item.Length);
                    if (double.TryParse(numText, out numberD))
                    {
                        var order = orders[item];
                        var newNumber = (double)(numberD * order);
                        return newNumber;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // 输入错误字符时
            return 0;
        }
    }
}
