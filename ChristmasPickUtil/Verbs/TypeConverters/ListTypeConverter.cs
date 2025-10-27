using ChristmasPickUtil.Verbs.Models;
using System.ComponentModel;
using System.Globalization;

namespace ChristmasPickUtil.Verbs.TypeConverters
{
    public class ListTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                if (Enum.TryParse(typeof(ListType), s, ignoreCase: true, out var result))
                {
                    return result!;
                }

                throw new ArgumentException($"'{s}' is not a valid ListType. Allowed values: {string.Join(", ", Enum.GetNames<ListType>())}");
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
