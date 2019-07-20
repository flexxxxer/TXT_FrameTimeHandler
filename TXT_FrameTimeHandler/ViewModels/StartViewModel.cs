using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXT_FrameTimeHandler.Commands;

namespace TXT_FrameTimeHandler.ViewModels
{
    public class StartViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string LogFilePath { get; set; }
        public string FrameTiminGraphFilePath { get; set; }
        public string ProbabilityDensityGraphFilePath { get; set; }
        public string ProbabilityDistributionGraphFilePath { get; set; }

        public ClassicCommand SelectLogFilePathCommand { get; }

        public StartViewModel()
        {
            this.SelectLogFilePathCommand = new ClassicCommand((arg) =>
            {

            }, (arg) => true);
        }
    }
}
