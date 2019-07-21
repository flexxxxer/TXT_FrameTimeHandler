using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace TXT_FrameTimeHandler.DataProcessing.FrameVew
{
    public static class FrameViewDataProcessing
    {
        public static Maybe<FramesData> ProcessFrameViewFile(string path)
        {
            FileStream fs = default;
            BufferedStream bs = default;
            StreamReader sr = default;

            try
            {
                var framesTimes = new LinkedList<double>();

                fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                bs = new BufferedStream(fs);
                sr = new StreamReader(bs);

                var MsBetweenDisplayChangeActualColumnIndex = sr.ReadLine()
                    .Split(',')
                    .ToList()
                    .FindIndex(column => column == "MsBetweenDisplayChangeActual");

                var line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Split(',')
                        [MsBetweenDisplayChangeActualColumnIndex];

                    var currentItemFrameTime = Convert.ToDouble(line,
                        CultureInfo.InvariantCulture // because Russians and others have "12,34", but not "12.34" :)
                        );
                    framesTimes.AddLast(currentItemFrameTime);
                }



                return new Maybe<FramesData>(
                    new FramesData(framesTimes)
                    );
            }
            catch (Exception)
            {
                if (fs != null)
                    fs.Dispose();
                if (bs != null)
                    bs.Dispose();
                if (sr != null)
                    sr.Dispose();

                return Maybe<FramesData>.None;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
                if (bs != null)
                    bs.Dispose();
                if (sr != null)
                    sr.Dispose();
            }
        }
    }
}
