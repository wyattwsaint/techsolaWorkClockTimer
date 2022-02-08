using System.ComponentModel;
using System.Windows;

namespace techsolaWorkClockTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TechsolaClock();
        }

        private void StartStopClock_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            clock.startStop();
        }
    }
}
