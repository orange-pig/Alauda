using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Alauda
{
    public class EnumToDescriptionStringConverterExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new EnumToDescriptionStringConverter();
        }
    }
}
