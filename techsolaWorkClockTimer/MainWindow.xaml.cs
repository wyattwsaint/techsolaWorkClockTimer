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

            if (!clock.IsRunning)
                clock.Start(TechsolaClock.DefaultProjectName);
            else
                clock.Stop();
        }

        private void EndOfDay_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;

            if (clock.IsRunning)
                clock.Stop();
        }

        private void ProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var clock = (TechsolaClock)DataContext;
            var projectTime = (ProjectTime)((Button)sender).DataContext;
            
            if (clock.Segments.Any(segment => segment.Project == projectTime.ProjectName && segment.End == null))
                return;

            if (clock.IsRunning)
                clock.Stop();

            clock.Start(projectTime.ProjectName);
        }
    }
}