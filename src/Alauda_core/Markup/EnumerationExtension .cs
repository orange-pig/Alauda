using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Alauda.Extension;

namespace Alauda
{
    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;
        private IEnumerable<EnumerationMember> members;

        public class EnumerationMember
        {
            public string Description { get; set; }
            public object Enumerator { get; set; }
            public object Value { get; set; }
        }

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }

        protected EnumerationExtension()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            members = GetAllValuesAndDescriptions(EnumType);
            return members;
        }


        public IEnumerable<EnumerationMember> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException("t must be an enum type");

            var enumValues = Enum.GetValues(t).Cast<Enum>();

            var s= (
              from Enum enumValue in enumValues
              select new EnumerationMember
              {
                  Value = Convert.ChangeType(enumValue, enumValue.GetTypeCode()),
                  Enumerator = enumValue,
                  Description = enumValue.GetDescription()
              }).ToList();
            return s;
        }
    }
}
