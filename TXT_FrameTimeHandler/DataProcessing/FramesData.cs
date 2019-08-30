using System.Collections.Generic;
using System.Linq;
using TXT_FrameTimeHandler.Commands;

namespace TXT_FrameTimeHandler.DataProcessing
{
    public class FramesData
    {
        public IEnumerable<double> FramesTimes { get; }

        public int Count => this.FramesTimes.Count();

        public double TimeTest { get; }

        public double OneTenthPercentFPS { get; }
        public double OnePercentFps { get; }
        public double FivePercentFPS { get; }
        public double FiftyPercentFPS { get; }
        public double AvgFPS { get; }

        public FramesData(IEnumerable<double> framesTimes)
        {
            this.FramesTimes = framesTimes;

            var oneTenthPercentFPS = 0.0;
            var onePercentFps = 0.0;
            var fivePercentFPS = 0.0;
            var fiftyPercentFPS = 0.0;
            var avgFPS = 0.0;
            var testTime = 0.0;

            #region calc 0.1, 1, 5, 50% and avg fps

            testTime = framesTimes.Sum();

            var oneTenthPercentTime = testTime / 1000.0; // 0.1%
            var onePercentTime = testTime / 100.0; // 1%
            var fivePercentTime = testTime / 20.0; // 5%
            var fiftyPercentTime = testTime / 2.0; // 50%

            var orderedFramesTimes = framesTimes.OrderByDescending(value => value).ToArray();

            var framesTimeSum = 0.0;
            var index = -1;

            while (framesTimeSum < oneTenthPercentTime)
                framesTimeSum += orderedFramesTimes[++index];

            framesTimeSum += orderedFramesTimes[index + 1];
            oneTenthPercentFPS = 1000.0 / orderedFramesTimes[index + 1];

            while (framesTimeSum < onePercentTime)
                framesTimeSum += orderedFramesTimes[++index];

            onePercentFps = 1000.0 / orderedFramesTimes[index];

            while (framesTimeSum < fivePercentTime)
                framesTimeSum += orderedFramesTimes[++index];

            fivePercentFPS = 1000.0 / orderedFramesTimes[index];

            while (framesTimeSum < fiftyPercentTime)
                framesTimeSum += orderedFramesTimes[++index];

            fiftyPercentFPS = 1000.0 / orderedFramesTimes[index];

            avgFPS = framesTimes.Count() / testTime * 1000.0;

            #endregion

            this.OneTenthPercentFPS = oneTenthPercentFPS.Round2();
            this.OnePercentFps = onePercentFps.Round2();
            this.FivePercentFPS = fivePercentFPS.Round2();
            this.FiftyPercentFPS = fiftyPercentFPS.Round2();
            this.AvgFPS = avgFPS.Round2();
            this.TimeTest = (testTime / 1000.0).Round2();
        }
    }
}