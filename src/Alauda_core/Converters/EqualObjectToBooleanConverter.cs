using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alauda
{
    public class EqualObjectToBooleanConverter : IValueConverter
    {
        private bool _isReversed;

        public bool IsReversed { get { return _isReversed; } set { _isReversed = value; } }


        public EqualObjectToBooleanConverter()
            : this(false)
        {

        }

        public EqualObjectToBooleanConverter(bool isReversed)
        {
            _isReversed = isReversed;
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == parameter || value.Equals(parameter)) ^ _isReversed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
