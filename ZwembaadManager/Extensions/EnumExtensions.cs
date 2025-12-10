using System;
using System.ComponentModel;
using System.Reflection;

namespace ZwembaadManager.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null)
                return string.Empty;

            FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            
            if (fieldInfo == null)
                return enumValue.ToString();

            DescriptionAttribute? descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            
            return descriptionAttribute?.Description ?? enumValue.ToString();
        }
    }
}