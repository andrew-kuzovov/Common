using System;
using System.ComponentModel;
using System.Linq;

namespace KUtil.Common
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum value)
        {
            var type = value.GetType();

            var values = value.ToString().Split(',');

            return values.Length == 1
                ? GetDescription(type, values[0].Trim())
                : values.Select(item => GetDescription(type, item.Trim())).Join(", ");
        }

        private static string GetDescription(Type enumType, string value)
        {
            var attribute = enumType
                .GetField(value)
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .FirstOrDefault();

            return attribute != null
                ? attribute.Description
                : value;
        }
    }
}
