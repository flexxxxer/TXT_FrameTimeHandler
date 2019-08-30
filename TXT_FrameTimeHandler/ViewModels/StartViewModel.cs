using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using TXT_FrameTimeHandler.Commands;
using TXT_FrameTimeHandler.DataProcessing;
using TXT_FrameTimeHandler.DataProcessing.FrameVew;
using TXT_FrameTimeHandler.DataProcessing.Fraps;

namespace TXT_FrameTimeHandler.ViewModels
{
    public class StartViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            this.SelectLogFilePathCommand?.RaiseCanExecuteChanged();
            this.SelectFrameTimingGraphFilePathCommand?.RaiseCanExecuteChanged();
            this.SelectProbabilityDensityGraphFilePathCommand?.RaiseCanExecuteChanged();
            this.SelectProbabilityDistributionGraphFilePathCommand?.RaiseCanExecuteChanged();

            this.OpenLogFileCommand?.RaiseCanExecuteChanged();
            this.SaveAsTxtFrameTimingGraphCommand?.RaiseCanExecuteChanged();
            this.SaveAsTxtProbabilityDensityGraphCommand?.RaiseCanExecuteChanged();
            this.SaveAsTxtProbabilityDistributionGraphCommand?.RaiseCanExecuteChanged();

            this.WriteFrameTimingGraphCommand?.RaiseCanExecuteChanged();
            this.WriteProbabilityDensityGraphCommand?.RaiseCanExecuteChanged();
            this.WriteProbabilityDistributionGraphCommand?.RaiseCanExecuteChanged();
        }

        private string _reportName = "";
        public string ReportName
        {
            get => this._reportName;
            set
            {
                if (this._reportName == value)
                    return;

                this._reportName = value;
                this.OnPropertyChanged("ReportName");
            }
        }

        private string _logFilePath;
        private string _frameTimingGraphFilePath;
        private string _probabilityDensityGraphFilePath;
        private string _probabilityDistributionGraphFilePath;
        private Maybe<FramesData> _resultFramesData = Maybe<FramesData>.None;

        public string LogFilePath
        {
            get => this._logFilePath;
            set
            {
                if (this._logFilePath == value)
                    return;

                this._logFilePath = value;
                this.OnPropertyChanged("LogFilePath");
            }
        }
        public string FrameTimingGraphFilePath
        {
            get => this._frameTimingGraphFilePath;
            set
            {
                if (this._frameTimingGraphFilePath == value)
                    return;

                this._frameTimingGraphFilePath = value;
                this.OnPropertyChanged("FrameTimingGraphFilePath");
            }
        }
        public string ProbabilityDensityGraphFilePath
        {
            get => this._probabilityDensityGraphFilePath;
            set
            {
                if (this._probabilityDensityGraphFilePath == value)
                    return;

                this._probabilityDensityGraphFilePath = value;
                this.OnPropertyChanged("ProbabilityDensityGraphFilePath");
            }
        }
        public string ProbabilityDistributionGraphFilePath
        {
            get => this._probabilityDistributionGraphFilePath;
            set
            {
                if (this._probabilityDistributionGraphFilePath == value)
                    return;

                this._probabilityDistributionGraphFilePath = value;
                this.OnPropertyChanged("ProbabilityDistributionGraphFilePath");
            }
        }
        public Maybe<FramesData> ResultFramesData
        {
            get => this._resultFramesData;
            set
            {
                if (this._resultFramesData == value)
                    return;

                this._resultFramesData = value;
                this.OnPropertyChanged("ResultFramesData");
            }
        }

        public ClassicCommand SelectLogFilePathCommand { get; }
        public ClassicCommand SelectFrameTimingGraphFilePathCommand { get; }
        public ClassicCommand SelectProbabilityDensityGraphFilePathCommand { get; }
        public ClassicCommand SelectProbabilityDistributionGraphFilePathCommand { get; }

        public ClassicCommand OpenLogFileCommand { get; }
        public ClassicCommand SaveAsTxtFrameTimingGraphCommand { get; }
        public ClassicCommand SaveAsTxtProbabilityDensityGraphCommand { get; }
        public ClassicCommand SaveAsTxtProbabilityDistributionGraphCommand { get; }

        /// <summary>
        /// Строит график времени кадра
        /// </summary>
        public ClassicCommand WriteFrameTimingGraphCommand { get; }

        /// <summary>
        /// Строит график плотности вероятности
        /// </summary>
        public ClassicCommand WriteProbabilityDensityGraphCommand { get; }

        /// <summary>
        /// Строит график распределения вероятности
        /// </summary>
        public ClassicCommand WriteProbabilityDistributionGraphCommand { get; }

        private Window ViewWindow => Application.Current.MainWindow;

        public StartViewModel()
        {
            this.SelectLogFilePathCommand = new ClassicCommand((arg) =>
            {
                var fileDialog = new OpenFileDialog
                {
                    Filter = "CSV Files|*.csv;|All Files|*.*"
                };

                if (Directory.GetFiles(Directory.GetCurrentDirectory()).Any(dirfile => Path.GetExtension(dirfile) == ".csv"))
                    fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

                var result = fileDialog.ShowDialog(this.ViewWindow);

                if (result != true)
                    return;

                var file = fileDialog.FileName;

                this.LogFilePath = file;

                this.OpenLogFileCommand.Execute(new object());

            }, (arg) => true);

            this.SelectFrameTimingGraphFilePathCommand = new ClassicCommand((arg) =>
            {
                var fileDialog = new OpenFileDialog
                {
                    Filter = "GRG Files|*.grf;|All Files|*.*"
                };

                var result = fileDialog.ShowDialog(this.ViewWindow);

                if (result != true)
                    return;

                var file = fileDialog.FileName;

                this.FrameTimingGraphFilePath = file;

            }, (arg) => true);

            this.SelectProbabilityDensityGraphFilePathCommand = new ClassicCommand((arg) =>
            {
                var fileDialog = new OpenFileDialog
                {
                    Filter = "GRG Files|*.grf;|All Files|*.*"
                };

                var result = fileDialog.ShowDialog(this.ViewWindow);

                if (result != true)
                    return;

                var file = fileDialog.FileName;

                this.ProbabilityDensityGraphFilePath = file;

            }, (arg) => true);

            this.SelectProbabilityDistributionGraphFilePathCommand = new ClassicCommand((arg) =>
            {
                var fileDialog = new OpenFileDialog
                {
                    Filter = "GRG Files|*.grf;|All Files|*.*"
                };

                var result = fileDialog.ShowDialog(this.ViewWindow);

                if (result != true)
                    return;

                var file = fileDialog.FileName;

                this.ProbabilityDistributionGraphFilePath = file;

            }, (arg) => true);

            this.OpenLogFileCommand = new ClassicCommand((arg) =>
            {
                Maybe<FramesData> result = FrapsDataProcessing.ProcessFrapsFile(this.LogFilePath);
                result = result.HasValue ? result : FrameViewDataProcessing.ProcessFrameViewFile(this.LogFilePath);
                this.ResultFramesData = result;

                if (!result.HasValue)
                {
                    MessageBox.Show("this is not Fraps or FrameView file");
                    return;
                }
                else
                {
                    FramesData data = result.Value;

                    var headerContent = "TXT FrameTimeHandler v0.5     Konushkov Pavel. YouTube Channel: Этот Компьютер"
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Frames\t\tTime test\t\tAVG_FPS\t\tLow 0.1% mFPS\t\tLow 1% mFPS\t\tLow 5% mFPS\t\tLow 50% mFPS\t\ttest name"
                    + Environment.NewLine;

                    var currentLogInfo = $"{data.Count}\t\t{data.TimeTest.MyToString()}s\t\t\t{data.AvgFPS.MyToString()}\t\t{data.OneTenthPercentFPS.MyToString()}\t\t\t{data.OnePercentFps.MyToString()}\t\t\t{data.FivePercentFPS.MyToString()}\t\t\t{data.FiftyPercentFPS.MyToString()}\t\t\t{this.ReportName}"
                            + Environment.NewLine;

                    if (File.Exists($"{Directory.GetCurrentDirectory()}\\data.txt"))
                        File.AppendAllText($"{Directory.GetCurrentDirectory()}\\data.txt",
                            currentLogInfo);
                    else
                        File.WriteAllText($"{Directory.GetCurrentDirectory()}\\data.txt", headerContent + currentLogInfo);
                }

            }, (arg) => !string.IsNullOrEmpty(this.ReportName) && !string.IsNullOrEmpty(this.LogFilePath) && File.Exists(this.LogFilePath));

            this.SaveAsTxtFrameTimingGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;

                // данные графика времени кадров
                var frames = new List<(double frameNumber, double frameTime)>(data.FramesTimes.Count() - 1);
                var frameNumber = 0.0;
                foreach (var frameTimeItem in data.FramesTimes.Skip(1))
                {
                    frameNumber += frameTimeItem;
                    frames.Add((frameNumber, frameTimeItem));
                }

                var content = string.Join(Environment.NewLine,
                    frames.Select(frame =>
                        string.Format(CultureInfo.InvariantCulture, "{0}, \t{1}",
                            frame.frameNumber.Round2(), frame.frameTime.Round2())
                        )
                    );

                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\FrameTimeReport_{this.ReportName}.txt"
                    , content);

            }, (arg) => this.ResultFramesData.HasValue && !string.IsNullOrEmpty(this.ReportName));

            this.SaveAsTxtProbabilityDensityGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;
                var testTime = data.TimeTest * 1000.0;
                var distrValues = new Dictionary<double, double>();

                IEnumerable<double> allFrameTimes = data.FramesTimes
                    .Select(value => value.Round05());


                foreach (var frameTime in
                    allFrameTimes.ElementAt(0).Round() == 0.0 ?
                        allFrameTimes.Skip(1) : allFrameTimes)
                {

                    if (distrValues.ContainsKey(frameTime))
                        distrValues[frameTime] += frameTime;
                    else
                        distrValues.Add(frameTime, frameTime);
                }


                IEnumerable<(double, double)> points =
                    distrValues.OrderByDescending(item => item.Key)
                        .Select(item => ((1000.0 / item.Key).Round2(), item.Value.Round2()));

                var content = string.Join(Environment.NewLine,
                    points.Select(frame =>
                        string.Format(CultureInfo.InvariantCulture, "{0}, \t{1}",
                            frame.Item1.Round2(), frame.Item2.Round2())
                        )
                    );

                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\ProbabilityDensityReport_{this.ReportName}.txt"
                    , content);

            }, (arg) => this.ResultFramesData.HasValue && !string.IsNullOrEmpty(this.ReportName));

            this.SaveAsTxtProbabilityDistributionGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;

                // данные графика плотности вероятности
                var frames = new List<(double frameValue, double frameTime)>(data.FramesTimes.Count() - 1);
                var sum_time = 0.0;
                foreach (var frameTimeItem in data.FramesTimes.OrderByDescending(v => v))
                {
                    sum_time += frameTimeItem;
                    var frameValue = 1000.0 / frameTimeItem;
                    frames.Add((frameValue, sum_time / data.TimeTest / 10.0));
                }

                var content = string.Join(Environment.NewLine,
                    frames.Select(frame =>
                        string.Format(CultureInfo.InvariantCulture, "{0}, \t{1}",
                            frame.frameValue.Round2(), frame.frameTime.Round2())
                        )
                    );

                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\ProbabilityDistributionReport_{this.ReportName}.txt"
                    , content);

            }, (arg) => this.ResultFramesData.HasValue && !string.IsNullOrEmpty(this.ReportName));

            this.WriteFrameTimingGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;
                var totalTime = data.TimeTest;
                // данные графика времени кадров
                var frames = new List<(double frameNumber, double frameTime)>(data.FramesTimes.Count() - 1);
                var frameNumber = 0.0;
                foreach (var frameTimeItem in data.FramesTimes.Skip(1))
                {
                    frameNumber += frameTimeItem;
                    frames.Add((frameNumber, frameTimeItem));
                }

                IEnumerable<(double, double)> points =
                    frames.Select(item => (item.frameNumber, item.frameTime));

                var textPointsContent = string.Join(";",
                    points
                        .Select(value => $"{value.Item1.MyToString()},{value.Item2.MyToString()}")
                    );

                Func<int, string> func = n => $@"[PointSeries{n}]
FillColor = clRed
LineColor = clRed
Size = 0
Style = 0
LineSize = 2
LineStyle = 0
Interpolation = 2
LabelPosition = 1
PointCount = {points.Count() + 1}
Points = {textPointsContent};
LegendText = {this.ReportName}";
                GraphApi.WriteNewGraphToFile(this.FrameTimingGraphFilePath, func);

            }, (arg) => this.ResultFramesData.HasValue && !string.IsNullOrEmpty(this.ReportName) && !string.IsNullOrEmpty(this.FrameTimingGraphFilePath) && File.Exists(this.LogFilePath));

            this.WriteProbabilityDensityGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;
                var testTime = data.TimeTest * 1000.0;
                var distrValues = new Dictionary<double, double>();

                IEnumerable<double> allFrameTimes = data.FramesTimes
                    .Select(value => value.Round05());


                foreach (var frameTime in
                    allFrameTimes.ElementAt(0).Round() == 0.0 ?
                        allFrameTimes.Skip(1) : allFrameTimes)
                {
                    if (distrValues.ContainsKey(frameTime))
                        distrValues[frameTime] += frameTime;
                    else
                        distrValues.Add(frameTime, frameTime);
                }

                IEnumerable<(double, double)> points =
                    distrValues.OrderByDescending(item => item.Key)
                        .Select(item => (
                            item.Key == 0.0 ? 0.0 : (1000.0 / item.Key).Round2(), item.Value.Round2()
                            )
                        );

                (double, double) min = points.Min();

                points = new[] { (Math.Truncate(min.Item1) - 1.0, 0.0) }.Concat(points);

                var textPointsContent = string.Join(";",
                    points.Select(value => $"{value.Item1.MyToString()},{value.Item2.MyToString()}")
                );

                Func<int, string> func = n => $@"[PointSeries{n}]
FillColor = clRed
LineColor = clRed
Size = 0
Style = 0
LineSize = 2
LineStyle = 0
Interpolation = 2
LabelPosition = 1
PointCount = {points.Count() + 1}
Points = {textPointsContent};
LegendText = {this.ReportName}";
                GraphApi.WriteNewGraphToFile(this.ProbabilityDensityGraphFilePath, func);

            }, (arg) => this.ResultFramesData.HasValue && !string.IsNullOrEmpty(this.ReportName) && !string.IsNullOrEmpty(this.ProbabilityDensityGraphFilePath) && File.Exists(this.LogFilePath));

            this.WriteProbabilityDistributionGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;

                // данные графика плотности вероятности
                var frames = new List<(double frameValue, double frameTime)>(data.FramesTimes.Count() - 1);
                var sumTime = 0.0;
                foreach (var frameTimeItem in data.FramesTimes.OrderByDescending(v => v))
                {
                    sumTime += frameTimeItem;
                    var frameValue = 1000.0 / frameTimeItem;
                    frames.Add((frameValue, sumTime / data.TimeTest / 10.0));
                }

                IEnumerable<(double, double)> points =
                    frames.Select(item => (item.frameValue.Round2(), item.frameTime.Round2()));

                var textPointsContent = string.Join(";",
                    points
                        .Select(value => $"{value.Item1.MyToString()},{value.Item2.MyToString()}")
                    );

                Func<int, string> func = n => $@"[PointSeries{n}]
FillColor = clRed
LineColor = clRed
Size = 0
Style = 0
LineSize = 2
LineStyle = 0
Interpolation = 2
LabelPosition = 1
PointCount = {points.Count() + 1}
Points = {textPointsContent};
LegendText = {this.ReportName}";
                GraphApi.WriteNewGraphToFile(this.ProbabilityDistributionGraphFilePath, func);

            }, (arg) => this.ResultFramesData.HasValue && !string.IsNullOrEmpty(this.ReportName) && !string.IsNullOrEmpty(this.ProbabilityDistributionGraphFilePath) && File.Exists(this.LogFilePath));
        }
    }
}