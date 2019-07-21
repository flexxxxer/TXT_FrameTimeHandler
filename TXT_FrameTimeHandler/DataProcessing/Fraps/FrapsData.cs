using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing.Fraps
{
    public class FrapsData
    {
        public LinkedList<double> FrameTimes { get; set; }

        public FrapsData()
        {
            this.FrameTimes = new LinkedList<double>();
        }
    }
}
