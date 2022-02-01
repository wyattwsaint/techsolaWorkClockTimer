using System;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        }

        private void startStopClock_Click(object sender, RoutedEventArgs e)
        {
            TechsolaClock clock = new TechsolaClock();
            clock.StartClock();
        }
    }
}
