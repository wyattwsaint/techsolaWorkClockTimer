using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

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

        private void StartPauseClock_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (!clock.IsRunning)
                clock.Start("Techsola Internal");
            else
                clock.Stop();
        }

        private void EndOfDay_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.IsRunning)
                clock.Stop();
        }

        private void Heritage_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            if (clock.IsRunning)
            {
                HeritageTimeElapsed.Visibility = Visibility.Visible;
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Heritage"));
            }
        }

        private void Exactis_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            if (clock.IsRunning)
            {
                ExactisTimeElapsed.Visibility = Visibility.Visible;
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Exactis"));
            }
        }

        private void CapriCork_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            if (clock.IsRunning)
            {
                CapriCorkTimeElapsed.Visibility = Visibility.Visible;
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Capri Cork"));
            }
        }
    }
}