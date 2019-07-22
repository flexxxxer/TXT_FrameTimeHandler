using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TXT_FrameTimeHandler.Commands
{
    public static class Util
    {
        public static double Round(this double value, int decimals) 
            => Math.Round(value, decimals);

        public static double Round2(this double value)
            => Math.Round(value, 2);
    }
}
