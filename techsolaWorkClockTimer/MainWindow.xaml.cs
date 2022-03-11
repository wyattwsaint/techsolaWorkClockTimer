using System.Linq;
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
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        private bool PauseOnLockoutBoxChecked { get; set; }

        private void StartPauseClock_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.RunningSegment is null)
                clock.Start(clock.Segments.LastOrDefault().Project);
            else
                clock.Stop();
        }

        private void EndOfDay_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.RunningSegment is not null)
                clock.Stop();

            clock.CreateEndOfDayWindow();
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
            if (!PauseOnLockoutBoxChecked) return;
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

        private void Pause_On_Lockout_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PauseOnLockoutBoxChecked = true;
        }
        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            PauseOnLockoutBoxChecked = false;
        }
    }
}