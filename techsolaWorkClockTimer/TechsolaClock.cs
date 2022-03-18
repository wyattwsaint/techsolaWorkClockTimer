using Dapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace techsolaWorkClockTimer
{
    public class TechsolaClock : ObservableObject
    {
        public TechsolaClock()
        {
            if (DataBase.DoesTableContainData() && DataBase.IsDataFromPriorDay())
                DataBase.RefreshTable();

            var timeSegments = DataBase.Connection.Query<TimeSegment>("select TimeSegmentStart, TimeSegmentEnd, Project from segments;");

            foreach (var timeSegment in timeSegments) segments.Add(timeSegment);
        }

        private CancellationTokenSource? cancellationTokenSource;
        private TimeSpan? totalTime;
        public string? DisplayTime => totalTime is not null ? $@"{totalTime:hh\:mm\:ss}" : null;
        public string? DecimalDisplayTime => totalTime is not null ? $@"{totalTime.Value.TotalHours:0.00}" : null;

        private string? breakTimeLeft;
        public string? BreakTimeLeft
        {
            get => breakTimeLeft;
            set => Set(ref breakTimeLeft, value);
        }
        
        public const string DefaultProjectName = "Techsola Internal";

        public ObservableCollection<ProjectTime> Times { get; } = new()
        {
            new(DefaultProjectName, "White"),
            new("Heritage", "PaleVioletRed"),
            new("Exactis", "PaleTurquoise"),
            new("Capri Cork", "PaleGoldenrod"),
            new("Zeager", "Peru"),
            new("JanTrak", "CadetBlue"),
            new("Traverse-Sales", "RosyBrown"),
            new("Traverse-Enhance", "YellowGreen"),
            new("WRA Database", "DarkGoldenrod"),
            new("DSB", "Turquoise"),
            new("General", "Thistle"),
            new("Custom", "Tomato"),
        };
        
        private readonly ObservableCollection<TimeSegment> segments = new();

        public ReadOnlyObservableCollection<TimeSegment> Segments => new(segments);

        public TimeSegment? RunningSegment => segments.LastOrDefault() is { End: null } runningSegment
            ? runningSegment
            : null;

        public void Start(string project)
        {
            if (RunningSegment is not null)
                throw new InvalidOperationException("Multiple segments must not run at the same time.");

            UpdateBreaktimeLeft(endOfDayTargetTime, workDayHours);

            segments.Add(new TimeSegment(DateTime.Now, project));
            OnPropertyChanged(nameof(RunningSegment));

            cancellationTokenSource = new();

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        totalTime = GetCurrentTime(project: null);
                        OnPropertyChanged(nameof(DisplayTime));
                        OnPropertyChanged(nameof(DecimalDisplayTime));
                        foreach (var projectTime in Times)
                            projectTime.Time = GetCurrentTime(projectTime.ProjectName);
                        await Task.Delay(250, cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                }
            });
        }

        public void Stop()
        {
            segments[^1].End = DateTime.Now;
            OnPropertyChanged(nameof(RunningSegment));

            cancellationTokenSource!.Cancel();

            cancellationTokenSource = null;
        }

        public TimeSpan GetCurrentTime(string? project)
        {
            return segments
                .Where(segment => project is null || segment.Project == project)
                .Sum(s => (s.End ?? DateTime.Now) - s.Start);
        }

        public void CreateEndOfDayWindow()
        {
            WorkdayComplete endOfDayPopUp = new();

            endOfDayPopUp.Visibility = Visibility.Visible;
        }

        private TimeSpan? endOfDayTargetTime;

        public void ConvertTimeIntArrayToTimeSpan(int[] targetTimeInt)
        {
            if (targetTimeInt.Length == 1)
            {
                endOfDayTargetTime = new TimeSpan(0, targetTimeInt[0] + 12, 0, 0);
            }
            if (targetTimeInt.Length == 2)
            {
                endOfDayTargetTime = new TimeSpan(0, targetTimeInt[0] + 12, targetTimeInt[1], 0);
            }
            UpdateBreaktimeLeft(endOfDayTargetTime, workDayHours);
        }

        private TimeSpan? workDayHours;

        public void GetWorkdayHoursFromComboBox(int targetHours)
        {
            workDayHours = new TimeSpan(0, targetHours, 0, 0);
            UpdateBreaktimeLeft(endOfDayTargetTime, workDayHours);
        }

        public void UpdateBreaktimeLeft(TimeSpan? endOfDay, TimeSpan? workHours)
        {
            if(endOfDayTargetTime == null && workDayHours == null) return;
            var breakTime = (endOfDay - DateTime.Now.TimeOfDay) -
                                        (workHours - GetCurrentTime(project: null));
            BreakTimeLeft = breakTime?.Ticks < 0 ? $@"-{breakTime:hh\:mm\:ss}" : $@"{breakTime:hh\:mm\:ss}";
        }

    }
}