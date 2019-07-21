using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing.FrameVew
{
    public static class FrameViewDataProcessing
    {
        public static Maybe<FramesData> ProcessFrapsFile()
        {
            try
            {
                var frameViewData = new FramesData(null);



                return new Maybe<FramesData>(frameViewData);
            }
            catch (Exception)
            {
                return Maybe<FramesData>.None;
            }
        }
    }
}
