using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alauda
{
    public class NumberUnitFormatConvertor : IValueConverter
    {
        private Dictionary<string, ulong> orders = new Dictionary<string, ulong>() {
            { "K", 1000 }, { "M", 1000000 }, { "G", 1000000000 } , { "T", 1000000000000 } , { "P", 1000000000000000 }  };

        private string _unit;

        public string Unit { get { return _unit; } set { _unit = value; } }

        public int DecimalDigit { get; set; }

        public bool HasIntervalBeforeUnit { get; set; }

        public NumberUnitFormatConvertor()
            : this("")
        {
        }

        public NumberUnitFormatConvertor(string unit)
        {
            DecimalDigit = 6;
            HasIntervalBeforeUnit = true;

            _unit = unit;

            var length = _unit.Length;
            if (length <= 0) return;

            for (var index = 1; index <= length; index++)
            {
                var suffix = _unit.Substring(0, index);

                orders.Add("K" + suffix, 1000);
                orders.Add("M" + suffix, 1000000);
                orders.Add("G" + suffix, 1000000000);
                orders.Add("T" + suffix, 1000000000000);
                orders.Add("P" + suffix, 1000000000000000);
            }

            orders.Add(_unit, 1);
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
            var unit = Unit;

            if (value < 1e3 && value > -1e3)
            {
                displayValue = value;
                devimalDigit = 0;
            }
            else if (value < 1e6 && value > -1e6)
            {
                displayValue = value / 1e3;
                devimalDigit = 3;
                unit = "K" + Unit;
            }
            else if (value < 1e9 && value > -1e9)
            {
                displayValue = value / 1e6;
                devimalDigit = 6;
                unit = "M" + Unit;
            }
            else if (value < 1e12 && value > -1e12)
            {
                displayValue = value / 1e9;
                devimalDigit = 6;
                unit = "G" + Unit;
            }
            else if (value < 1e15 && value > -1e15)
            {
                displayValue = value / 1e12;
                devimalDigit = 6;
                unit = "T" + Unit;
            }
            else if (value < 1e18 && value > -1e18)
            {
                displayValue = value / 1e15;
                devimalDigit = 6;
                unit = "P" + Unit;
            }
            else
            {
                displayValue = 1e18;// 用旧值重新覆盖
            }

            if (devimalDigit > DecimalDigit)
            {
                devimalDigit = DecimalDigit;
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                return string.Format("{0:F" + devimalDigit + "}", displayValue);
            }
            else
            {
                if (HasIntervalBeforeUnit)
                {
                    return string.Format("{0:F" + devimalDigit + "} {1}", displayValue, unit);
                }
                else
                {
                    return string.Format("{0:F" + devimalDigit + "}{1}", displayValue, unit);
                }
            }
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
