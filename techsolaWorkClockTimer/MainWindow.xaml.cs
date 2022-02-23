using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

            if (clock.RunningSegment?.Project == projectTime.ProjectName)
            {
                clock.Stop();
                clock.Start(TechsolaClock.DefaultProjectName);
                return;
            }

            if (clock.RunningSegment is not null)
                clock.Stop();

            clock.Start(projectTime.ProjectName);
        }
    }
}