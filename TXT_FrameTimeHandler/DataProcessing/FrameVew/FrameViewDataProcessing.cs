using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using TXT_FrameTimeHandler.Commands;

namespace TXT_FrameTimeHandler.DataProcessing.FrameVew
{
    public static class FrameViewDataProcessing
    {
        public static Maybe<FramesData> ProcessFrameViewFile(string path)
        {
            var fs = default(FileStream);
            var bs = default(BufferedStream);
            var sr = default(StreamReader);

            try
            {
                var framesTimes = new LinkedList<double>();

                fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                bs = new BufferedStream(fs);
                sr = new StreamReader(bs);

                var MsBetweenDisplayChangeActualColumnIndex = 
                    sr.ReadLine() // maybe null reference exception
                    .Split(',')
                    .ToList()
                    .FindIndex(column => column == "MsBetweenDisplayChangeActual" || column == "MsBetweenDisplayChange");

                var line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Split(',')
                        .ElementAt(MsBetweenDisplayChangeActualColumnIndex);

                    var currentItemFrameTime = Convert.ToDouble(line,
                        CultureInfo.InvariantCulture // because Russians and others have "12,34", but not "12.34" :)
                        ).Round2();
                    framesTimes.AddLast(currentItemFrameTime);
                }

                return new Maybe<FramesData>(
                    new FramesData(framesTimes)
                    );
            }
            catch { /* ignore */ }
            finally
            {
                fs?.Dispose();
                bs?.Dispose();
                sr?.Dispose();
            }

            // if we catch exception while reading, return none
            return Maybe<FramesData>.None;
        }
    }
}