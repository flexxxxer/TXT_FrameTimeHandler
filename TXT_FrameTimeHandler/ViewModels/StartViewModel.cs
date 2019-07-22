using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ClassicCommand WriteFrameTimingGraphCommand { get; }
        public ClassicCommand WriteProbabilityDensityGraphCommand { get; }
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
                    var content = "TXT FrameTimeHandler v0.5     Konushkov Pavel. YouTube Channel: Этот Компьютер" 
                    + Environment.NewLine 
                    + Environment.NewLine 
                    + "Frames\t\tTime test\t\tAVG_FPS\t\tLow 0.1% mFPS\t\tLow 1% mFPS\t\tLow 5% mFPS\t\tLow 50% mFPS\t\ttest name"
                    + Environment.NewLine
                    + $"{data.Count}\t\t{data.TimeTest}\t\t{data.AvgFPS}\t\t{data.OneTenthPercentFPS}\t\t\t{data.OnePercentFps}\t\t\t{data.FivePercentFPS}\t\t\t{data.FiftyPercentFPS}\t\t\t{"my test"}"
                    + Environment.NewLine;

                    File.WriteAllText($"{Directory.GetCurrentDirectory()}\\data.txt", content);
                }

            }, (arg) => string.IsNullOrEmpty(this.LogFilePath) ? false : File.Exists(this.LogFilePath));

            this.SaveAsTxtFrameTimingGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;

                // данные графика времени кадров
                var frames = new List<(double frameNumber, double frameTime)>(data.FramesTimes.Count() - 1);
                var frameNumber = 1;
                foreach (var frameTimeItem in data.FramesTimes.Skip(1))
                {
                    var frameN = frameNumber++ + frameTimeItem;
                    frames.Add( (frameN, frameTimeItem) );
                }

                var content = string.Join(Environment.NewLine,
                    frames.Select(frame => 
                        string.Format(CultureInfo.InvariantCulture, "{0}, \t{1}", 
                            frame.frameNumber.Round2(), frame.frameTime.Round2())
                        )
                    );

                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\FrameTimeReport.txt"
                    , content);

            }, (arg) => this.ResultFramesData.HasValue);

            this.SaveAsTxtProbabilityDensityGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;

                // данные графика плотности вероятности
                var frames = new List<(double frameValue, double frameTime)>(data.FramesTimes.Count() - 1);
                var sum_time = 0.0;
                foreach (var frameTimeItem in data.FramesTimes.Skip(1))
                {
                    sum_time += frameTimeItem;
                    var frameValue = 1000.0 / frameTimeItem;
                    frames.Add( (frameValue, sum_time / data.TimeTest * 100) );
                }

                var content = string.Join(Environment.NewLine,
                    frames.Select(frame =>
                        string.Format(CultureInfo.InvariantCulture, "{0}, \t{1}",
                            frame.frameValue.Round2(), frame.frameTime.Round2())
                        )
                    );

                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\ProbabilityDensityReport.txt"
                    , content);

            }, (arg) => this.ResultFramesData.HasValue);

            this.SaveAsTxtProbabilityDistributionGraphCommand = new ClassicCommand((arg) =>
            {
                FramesData data = this.ResultFramesData.Value;

                // данные графика распределения вероятности
                var frames = new List<(double frameValue, double frameTime)>(data.FramesTimes.Count() - 1);
                var frameNumber = 1;
                foreach (var frameTimeItem in data.FramesTimes.Skip(1))
                {
                    var frameValue = 1000.0 / frameNumber++;
                    frames.Add( (frameValue, frameTimeItem) );
                }

                var content = string.Join(Environment.NewLine,
                    frames.Select(frame =>
                        string.Format(CultureInfo.InvariantCulture, "{0}, \t{1}",
                            frame.frameValue.Round2(), frame.frameTime.Round2())
                        )
                    );

                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\ProbabilityDistributionReport.txt"
                    , content);

            }, (arg) => this.ResultFramesData.HasValue);

            this.WriteFrameTimingGraphCommand = new ClassicCommand((arg) =>
            {

            }, (arg) => this.ResultFramesData.HasValue && (string.IsNullOrEmpty(this.FrameTimingGraphFilePath) ? false : File.Exists(this.LogFilePath)));

            this.WriteProbabilityDensityGraphCommand = new ClassicCommand((arg) =>
            {

            }, (arg) => this.ResultFramesData.HasValue && (string.IsNullOrEmpty(this.ProbabilityDensityGraphFilePath) ? false : File.Exists(this.LogFilePath)));

            this.WriteProbabilityDistributionGraphCommand = new ClassicCommand((arg) =>
            {

            }, (arg) => this.ResultFramesData.HasValue && (string.IsNullOrEmpty(this.ProbabilityDistributionGraphFilePath) ? false : File.Exists(this.LogFilePath)));
        }
    }
}
