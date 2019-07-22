using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using TXT_FrameTimeHandler.Commands;

namespace TXT_FrameTimeHandler.DataProcessing.Fraps
{
    public static class FrapsDataProcessing
    {
        public static Maybe<FramesData> ProcessFrapsFile(string path)
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

                sr.ReadLine(); // read emmpty line
                sr.ReadLine(); // read first item

                var line = "";
                var lastItemFrameTime = 0.0; // first item frametime is always zero 

                framesTimes.AddLast(lastItemFrameTime);

                // others
                while ((line = sr.ReadLine()) != null)
                {
                    #region parse string of format "framenumber, frametime"
                    var index = line.LastIndexOf(',');
                    line = line.Substring(index + 1);
                    line = string
                        .Join("", line.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                    #endregion

                    var currentItemFrameTime = Convert.ToDouble(line, 
                        CultureInfo.InvariantCulture // because Russians and others have "12,34", but not "12.34" :)
                        );
                    framesTimes.AddLast( (currentItemFrameTime - lastItemFrameTime).Round2());
                    lastItemFrameTime = currentItemFrameTime;
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
