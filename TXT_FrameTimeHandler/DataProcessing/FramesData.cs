using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing
{
    public class FramesData
    {
        public IEnumerable<double> FramesTimes { get; }

        public FramesData(IEnumerable<double> framesTimes)
        {
            this.FramesTimes = framesTimes;
        }
    }
}
