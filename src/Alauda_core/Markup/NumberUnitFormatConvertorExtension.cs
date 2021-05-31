using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Alauda
{
    public class NumberUnitFormatConvertorExtension : MarkupExtension
    {
        private string _unit;
        private int _decimalDigit;
        private bool _hasIntervalBeforeUnit;

        [ConstructorArgument("unit")]
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        [ConstructorArgument("decimalDigit")]
        public int DecimalDigit
        {
            get { return _decimalDigit; }
            set { _decimalDigit = value; }
        }

        [ConstructorArgument("hasIntervalBeforeUnit")]
        public bool HasIntervalBeforeUnit
        {
            get { return _hasIntervalBeforeUnit; }
            set { _hasIntervalBeforeUnit = value; }
        }


        public NumberUnitFormatConvertorExtension()
            : this("")
        {
        }

        public NumberUnitFormatConvertorExtension(string unit)
        {
            this.Unit = unit;
            DecimalDigit = 6;
            HasIntervalBeforeUnit = true;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new NumberUnitFormatConvertor(_unit)
            {
                DecimalDigit = this._decimalDigit,
                HasIntervalBeforeUnit = this._hasIntervalBeforeUnit
            };
        }
    }
}
