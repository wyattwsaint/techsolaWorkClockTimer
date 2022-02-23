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
                return; //To keep from adding new time start time segment property if Heritage clock is already running

            if (clock.IsRunning)
            {
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Heritage"));
                clock.IsHeritageRunning = true;

                if (clock.IsExactisRunning)
                {
                    clock.Segments.FindLast(segment => segment.Project == "Exactis")!.End = DateTime.Now;
                    clock.IsExactisRunning = false;
                }

                if (clock.IsCapriCorkRunning)
                {
                    clock.Segments.FindLast(segment => segment.Project == "Capri Cork")!.End = DateTime.Now;
                    clock.IsCapriCorkRunning = false;
                }
            }
        }

        private void Exactis_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.Segments.Where(segment => segment.Project == "Exactis")
                .Any(segment => segment.End == null)) return;

            if (clock.IsRunning)
            {
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Exactis"));
                clock.IsExactisRunning = true;

                if (clock.IsHeritageRunning)
                {
                    clock.Segments.FindLast(segment => segment.Project == "Heritage")!.End = DateTime.Now;
                    clock.IsHeritageRunning = false;
                }

                if (clock.IsCapriCorkRunning)
                {
                    clock.Segments.FindLast(segment => segment.Project == "Capri Cork")!.End = DateTime.Now;
                    clock.IsCapriCorkRunning = false;
                }
            }
        }

        private void CapriCork_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.Segments.Where(segment => segment.Project == "Capri Cork")
                .Any(segment => segment.End == null)) return;

            if (clock.IsRunning)
            {
                clock.Segments.Add(new TimeSegment(DateTime.Now, "Capri Cork"));
                clock.IsCapriCorkRunning = true;

                if (clock.IsHeritageRunning)
                {
                    clock.Segments.FindLast(segment => segment.Project == "Heritage")!.End = DateTime.Now;
                    clock.IsHeritageRunning = false;
                }

                if (clock.IsExactisRunning)
                {
                    clock.Segments.FindLast(segment => segment.Project == "Exactis")!.End = DateTime.Now;
                    clock.IsExactisRunning = false;
                }
            }
        }
    }
}