using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing.FrameVew
{
    public static class FrameViewDataProcessing
    {
        public static Maybe<FrameViewData> ProcessFrapsFile()
        {
            try
            {
                var frameViewData = new FrameViewData();



                return new Maybe<FrameViewData>(frameViewData);
            }
            catch (Exception)
            {
                return Maybe<FrameViewData>.None;
            }
        }
    }
}
