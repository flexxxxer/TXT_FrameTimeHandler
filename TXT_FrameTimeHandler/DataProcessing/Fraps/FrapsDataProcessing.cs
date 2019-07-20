using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing.Fraps
{
    public static class FrapsDataProcessing
    {
        public static Maybe<FrapsData> ProcessFrapsFile(string path)
        {
            return new Maybe<FrapsData>(new FrapsData());
        }
    }
}
