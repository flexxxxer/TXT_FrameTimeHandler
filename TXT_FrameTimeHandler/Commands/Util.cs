using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            var temp = Math.Truncate(value);

            if (temp > value)
                return temp - 0.5;
            else
                return temp + 0.5;
        }

    }
}
