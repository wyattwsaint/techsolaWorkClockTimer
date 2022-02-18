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

            if (clock.Segments.Where(segment => segment.Project == "Heritage").Any(segment => segment.End == null))
                return; //To keep from adding new time start time segments

            if (clock.IsRunning)
            {
                HeritageTimeElapsed.Visibility = Visibility.Visible;
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Heritage"));
                clock.IsHeritageRunning = true;
                clock.IsExactisRunning = false;
                clock.IsCapriCorkRunning = false;
            }
        }

        private void Exactis_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.Segments.Where(segment => segment.Project == "Exactis")
                .Any(segment => segment.End == null)) return;

            if (clock.IsRunning)
            {
                ExactisTimeElapsed.Visibility = Visibility.Visible;
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Exactis"));
                clock.IsExactisRunning = true;
                clock.IsCapriCorkRunning = false;
                clock.IsHeritageRunning = false;
            }
        }

        private void CapriCork_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            
            if (clock.Segments.Where(segment => segment.Project == "Capri Cork")
                .Any(segment => segment.End == null)) return;

            if (clock.IsRunning)
            {
                CapriCorkTimeElapsed.Visibility = Visibility.Visible;
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Capri Cork"));
                clock.IsCapriCorkRunning = true;
                clock.IsExactisRunning = false;
                clock.IsHeritageRunning = false;
            }
        }
    }
}