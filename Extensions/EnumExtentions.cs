using System.ComponentModel;
using System.Reflection;

namespace Extensions
{
    public static class EnumExtentions
    {
        public static string ToDescriptionString<TEnum>(this TEnum enumValue)
        {
            FieldInfo info = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            /*
            The ?. operator (a null-conditional operator) checks if the value is null before accessing its property.
            The ?? operator (a null-coalescing operator) tells the application to return the value at the left if it’s not empty, or the value at right otherwise.
            */
            return attributes?[0].Description ?? enumValue.ToString();
        }
    }
}