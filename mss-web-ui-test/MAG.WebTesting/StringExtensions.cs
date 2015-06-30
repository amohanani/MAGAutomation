using System;

namespace MAG.WebTesting
{
    public static class StringExtensions
    {
        public static TEnum ToEnum<TEnum>(this string input)
        {
            var t = typeof (TEnum);
            if (t.IsEnum)
            {
                return (TEnum)Enum.Parse(t, input);
            }

            throw new ArgumentException("This is not an enum");
        }
    }
}