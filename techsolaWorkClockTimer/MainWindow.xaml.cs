using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

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
            DataContext = App.Clock;
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            RefreshSegmentsTable refreshDb = new();
            if (refreshDb.IsDbEmpty())
            {
                refreshDb.RefreshTable();
            }
        }

        private void StartPauseClock_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.RunningSegment is null)
                clock.Start(TechsolaClock.DefaultProjectName);
            else
                clock.Stop();
        }

        private void EndOfDay_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.RunningSegment is not null)
                clock.Stop();
        }

        private void ProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            var projectTime = (ProjectTime)((Button)sender).DataContext;

            var wasSameProjectRunning = clock.RunningSegment?.Project == projectTime.ProjectName;

            if (clock.RunningSegment is not null)
                clock.Stop();

            clock.Start(wasSameProjectRunning
                ? TechsolaClock.DefaultProjectName
                : projectTime.ProjectName);
        }

        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    if (clock.RunningSegment is null)
                        clock.Start(TechsolaClock.DefaultProjectName);
                    else
                        clock.Stop();
                    break;

                case SessionSwitchReason.SessionUnlock:
                    if (clock.RunningSegment is null)
                        clock.Start(TechsolaClock.DefaultProjectName);
                    else
                        clock.Stop();
                    break;
            }
        }
    }
}