using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TXT_FrameTimeHandler.DataProcessing
{
    public static class GraphApi
    {
        public static string CreateDefaultHeader() => $@"; This file was created by Graph (http://www.padowan.dk)
; Do not change this file from other programs.
[Graph]
Version = 4.4.2.543
MinVersion = 2.5
OS = Windows NT 6.2

[Axes]
xMin = -5
xMax = 180
xTickUnit = 5
xGridUnit = 5
xShowGrid = 1
xLabel = mFPS
yMin = -600
yMax = 10000
yTickUnit = 500
yGridUnit = 3000
yShowGrid = 1
yAutoGrid = 0
yLabel = Время
yShowNumbers = 0
AxesColor = clBlack
GridColor = clSilver
NumberFont = Kizo Light,20,clBlack
LabelFont = Kizo Light,30,clBlack
LegendFont = Kizo Light,20,clBlack
ShowLegend = 1
Radian = 1
Title = DEFAULT TITLE
TitleFont = Kizo Light,30,clBlack";

        public static Maybe<int> GetPointSeriesCount(string path)
        {
            var fs = default(FileStream);
            var bs = default(BufferedStream);
            var sr = default(StreamReader);

            try
            {
                fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                bs = new BufferedStream(fs);
                sr = new StreamReader(bs);
                var line = string.Empty;

                while((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("PointSeriesCount"))
                        return new Maybe<int>(int.Parse(
                            line.Split('=') [1].Replace(" ", string.Empty)
                            )
                        );
                }

                return Maybe<int>.None;
            }
            catch
            {
                if (fs != null)
                    fs.Dispose();
                if (bs != null)
                    bs.Dispose();
                if (sr != null)
                    sr.Dispose();

                return Maybe<int>.None;
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

        public static string CreateDefaultDataWithPSCount(int count) => $@"[Data]
TextLabelCount = 0
FuncCount = 0
PointSeriesCount = {count}
ShadeCount = 0
RelationCount = 0
OleObjectCount = 0";

        public static void WriteNewGraphToFile(string path, Func<int, string> createGraphWithNumberFunc)
        {
            Maybe<int> psCount = GraphApi.GetPointSeriesCount(path);

            if(psCount.HasValue == false)
            {
                var content = GraphApi.CreateDefaultHeader() + Environment.NewLine + Environment.NewLine +
                    createGraphWithNumberFunc(1) + Environment.NewLine + Environment.NewLine +
                    GraphApi.CreateDefaultDataWithPSCount(1);

                File.WriteAllText(path, content, Encoding.UTF8);
            }
            else
            {
                var number = psCount.Value + 1;

                var content = string.Join(
                    Environment.NewLine, File.ReadLines(path).TakeWhile(s => s != "[Data]").ToArray()
                ) + Environment.NewLine + Environment.NewLine + createGraphWithNumberFunc(number) + Environment.NewLine +
                    Environment.NewLine + GraphApi.CreateDefaultDataWithPSCount(number);

                File.WriteAllText(path, content, Encoding.UTF8);
            }
        }
    }
}