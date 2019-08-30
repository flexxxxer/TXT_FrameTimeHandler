using System;
using System.Globalization;

namespace TXT_FrameTimeHandler.Commands
{
    public static class Util
    {
        public static double Round(this double value, int decimals = 0)
            => Math.Round(value, decimals);

        public static double Round2(this double value)
            => Math.Round(value, 2);

        public static string MyToString(this double value)
            => value.ToString(CultureInfo.InvariantCulture);

        public static double Round05(this double value)
        {
            var first2DecimalPlaces = (int)((decimal)value % 1 * 100);

            if (first2DecimalPlaces < 25)
                return value.Round();
            else if (first2DecimalPlaces < 75)
                return (long)value + 0.5;
            else
                return value.Round();
        }

    }
}