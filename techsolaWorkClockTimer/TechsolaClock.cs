using Dapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Accessibility;

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

            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);

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
        public TimeSpan? EndOfDayTargetTime
        {
            get => endOfDayTargetTime;
            set => Set(ref endOfDayTargetTime, value);
        }

        public void ConvertTimeIntArrayToTimeSpan(int[] targetTimeInt)
        {
            if (targetTimeInt.Length == 1)
            {
                EndOfDayTargetTime = new TimeSpan(0, targetTimeInt[0] + 12, 0, 0);
            }
            if (targetTimeInt.Length == 2)
            {
                EndOfDayTargetTime = new TimeSpan(0, targetTimeInt[0] + 12, targetTimeInt[1], 0);
            }
            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);
        }

        private TimeSpan? workDayHours;
        public TimeSpan? WorkDayHours
        {
            get => workDayHours;
            set => Set(ref workDayHours, value);
        }
        public void GetWorkdayHoursFromComboBox(int targetHours)
        {
            WorkDayHours = new TimeSpan(0, targetHours, 0, 0);
            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);
        }

        public string UpdateBreaktimeLeft(TimeSpan? endOfDay, TimeSpan? workHours)
        {
            if(endOfDayTargetTime == null && workDayHours == null) return "";
            var breakTime = (endOfDay - DateTime.Now.TimeOfDay) -
                                        (workHours - GetCurrentTime(project: null));
            return BreakTimeLeft = breakTime?.Ticks < 0 ? $@"-{breakTime:hh\:mm\:ss}" : $@"{breakTime:hh\:mm\:ss}";
        }

        private string? getEndOfDayTargetTime;
        public string? GetEndOfDayTargetTime
        {
            get => getEndOfDayTargetTime;
            set => Set(ref getEndOfDayTargetTime, value);
        }
        private string? getWorkDayHours;

        public string? GetWorkDayHours
        {
            get => getWorkDayHours;
            set => Set(ref getWorkDayHours, value);
        }

        private string ConvertTimeSpansToStringsForComboboxTwoWayBinding(TimeSpan time, bool addPm)
        {
            if (addPm)
            {
                var endOfDayTime = Convert.ToInt32(time.ToString().Substring(0, 2));
                if (endOfDayTime > 12) return (endOfDayTime - 12) + "PM";
                return endOfDayTime + "PM";
            }
            var workDayTime = Convert.ToInt32(time.ToString().Substring(0,2));
            if (workDayTime > 12) return (workDayTime - 12) + " HRS";
            return workDayTime.ToString();
        }

        public void GetSettings()
        {
            WorkDayHours = Properties.Settings.Default.workDayHours;
            EndOfDayTargetTime = Properties.Settings.Default.endOfDayTargetTime;

            getWorkDayHours =
                ConvertTimeSpansToStringsForComboboxTwoWayBinding(Properties.Settings.Default.workDayHours, addPm: false);
            OnPropertyChanged(nameof(GetWorkDayHours));

            getEndOfDayTargetTime =
                ConvertTimeSpansToStringsForComboboxTwoWayBinding(Properties.Settings.Default.endOfDayTargetTime, addPm: true);
            OnPropertyChanged(nameof(GetEndOfDayTargetTime));

            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);
        }
        public void SetSettings()
        {
            Properties.Settings.Default.endOfDayTargetTime = EndOfDayTargetTime!.Value;
            Properties.Settings.Default.workDayHours = WorkDayHours!.Value;
            Properties.Settings.Default.Save();
        }
    }
}