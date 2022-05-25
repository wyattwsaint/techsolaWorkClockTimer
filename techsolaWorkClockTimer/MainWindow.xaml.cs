using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace techsolaWorkClockTimer;

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
        {
            clock.Start(clock.Segments.Count != 0
                ? clock.Segments.LastOrDefault()!.Project
                : TechsolaClock.DefaultProjectName, clock.RunningSegment?.WorkItem, clock.EmployeeNumberTechClock);
        }
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
            : projectTime.ProjectName, null, clock.EmployeeNumberTechClock);
        //DevOpsApi.GetProjects();
    }

    void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        if (!PauseOnLockoutBoxChecked) return;
        var clock = (TechsolaClock)DataContext;
        switch (e.Reason)
        {
            case SessionSwitchReason.SessionLock:
                if (clock.RunningSegment is null)
                    clock.Start(TechsolaClock.DefaultProjectName, clock.RunningSegment?.WorkItem, clock.EmployeeNumberTechClock);
                else
                    clock.Stop();
                break;

            case SessionSwitchReason.SessionUnlock:
                if (clock.RunningSegment is null)
                    clock.Start(TechsolaClock.DefaultProjectName, clock.RunningSegment?.WorkItem, clock.EmployeeNumberTechClock);
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

    private void EndOfWorkDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;
        var targetTimeString = ((ComboBoxItem)daysEndTargetTime.SelectedItem).Content.ToString()!.TrimEnd('P', 'M')
            .Split(':');
        var targetTimeInt = Array.ConvertAll(targetTimeString, s => int.Parse(s));
        clock.ConvertTimeIntArrayToTimeSpan(targetTimeInt);
    }

    private void WorkDayLength_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;

        var comboBoxString =
            ((ComboBoxItem)workDayLength.SelectedItem).Content.ToString()!.TrimEnd(' ', 'H', 'R', 'S');
        var targetHours = Convert.ToInt32(comboBoxString);
        clock.GetWorkdayHoursFromComboBox(targetHours);
    }

    private void WorkItemOne_Click(object sender, RoutedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;
        var projectTime = (ProjectTime)((MenuItem)sender).DataContext;

        var wasSameProjectRunning = clock.RunningSegment?.Project == projectTime.ProjectName;

        if (clock.RunningSegment is not null)
            clock.Stop();

        clock.Start(wasSameProjectRunning
            ? TechsolaClock.DefaultProjectName
            : projectTime.ProjectName, clock.WorkItemOneTechsolaClock, clock.EmployeeNumberTechClock);
    }

    private void WorkItemTwo_Click(object sender, RoutedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;
        var projectTime = (ProjectTime)((MenuItem)sender).DataContext;

        var wasSameProjectRunning = clock.RunningSegment?.Project == projectTime.ProjectName;

        if (clock.RunningSegment is not null)
            clock.Stop();

        clock.Start(wasSameProjectRunning
            ? TechsolaClock.DefaultProjectName
            : projectTime.ProjectName, clock.WorkItemTwoTechsolaClock, clock.EmployeeNumberTechClock);
    }

    private void WorkItemThree_Click(object sender, RoutedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;
        var projectTime = (ProjectTime)((MenuItem)sender).DataContext;

        var wasSameProjectRunning = clock.RunningSegment?.Project == projectTime.ProjectName;

        if (clock.RunningSegment is not null)
            clock.Stop();

        clock.Start(wasSameProjectRunning
            ? TechsolaClock.DefaultProjectName
            : projectTime.ProjectName, clock.WorkItemThreeTechsolaClock, clock.EmployeeNumberTechClock);
    }

    private void WorkItemFour_Click(object sender, RoutedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;
        var projectTime = (ProjectTime)((MenuItem)sender).DataContext;

        var wasSameProjectRunning = clock.RunningSegment?.Project == projectTime.ProjectName;

        if (clock.RunningSegment is not null)
            clock.Stop();

        clock.Start(wasSameProjectRunning
            ? TechsolaClock.DefaultProjectName
            : projectTime.ProjectName, clock.WorkItemFourTechsolaClock, clock.EmployeeNumberTechClock);
    }

    private void WorkItemFive_Click(object sender, RoutedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;
        var projectTime = (ProjectTime)((MenuItem)sender).DataContext;

        var wasSameProjectRunning = clock.RunningSegment?.Project == projectTime.ProjectName;

        if (clock.RunningSegment is not null)
            clock.Stop();

        clock.Start(wasSameProjectRunning
            ? TechsolaClock.DefaultProjectName
            : projectTime.ProjectName, clock.WorkItemFiveTechsolaClock, clock.EmployeeNumberTechClock);
    }

    private void WorkItemSix_Click(object sender, RoutedEventArgs e)
    {
        var clock = (TechsolaClock)DataContext;
        var projectTime = (ProjectTime)((MenuItem)sender).DataContext;

        var wasSameProjectRunning = clock.RunningSegment?.Project == projectTime.ProjectName;

        if (clock.RunningSegment is not null)
            clock.Stop();

        clock.Start(wasSameProjectRunning
            ? TechsolaClock.DefaultProjectName
            : projectTime.ProjectName, clock.WorkItemSixTechsolaClock, clock.EmployeeNumberTechClock);
    }
}