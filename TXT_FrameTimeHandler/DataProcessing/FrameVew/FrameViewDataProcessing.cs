using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing.FrameVew
{
    public static class FrameViewDataProcessing
    {
        public static Maybe<FrameViewData> ProcessFrapsFile(string path)
        {
            return new Maybe<FrameViewData>(new FrameViewData());
        }
    }
}
