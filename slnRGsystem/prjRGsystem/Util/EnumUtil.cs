using System.ComponentModel;

namespace RootMvcProjectlib.Util
{
    public class EnumUtil
    {
        public static T Convert<T>(string? input, T defaultValue)
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        object? convertedValue = converter.ConvertFromString(input);
                        if (convertedValue != null)
                        {
                            return (T)convertedValue;
                        }
                        else
                        {
                            return defaultValue;
                        }
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Can't convert '{0}' to type [{1}]", input, typeof(T)));
                    }
                }
                catch (NotSupportedException)
                {
                    throw new ApplicationException(string.Format("Can't convert '{0}' to type [{1}]", input, typeof(T)));
                }
            }
            else
            {
                return defaultValue;
            }
        }

    }
}
