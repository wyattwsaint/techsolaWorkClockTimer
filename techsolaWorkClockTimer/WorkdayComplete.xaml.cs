using System.Windows;

namespace techsolaWorkClockTimer
{
    /// <summary>
    /// Interaction logic for WorkdayComplete.xaml
    /// </summary>
    public partial class WorkdayComplete : Window
    {
        public WorkdayComplete()
        {
            InitializeComponent();
            DataContext = App.Clock;

            var clock = (TechsolaClock)DataContext;

            clock.TotalTime = $@"{clock.GetCurrentTime(project: null).TotalHours:0.00}";
            foreach (var projectTime in clock.Times)
                projectTime.TotalTime = $@"{clock.GetCurrentTime(projectTime.ProjectName).TotalHours:0.00}";
        }
    }
}
