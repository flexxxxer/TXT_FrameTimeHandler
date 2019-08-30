using System.Windows;
using System.Windows.Input;

namespace TXT_FrameTimeHandler
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e) => this.Close();

        private void Minimize(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void MoveForm(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }

        }
    }
}
