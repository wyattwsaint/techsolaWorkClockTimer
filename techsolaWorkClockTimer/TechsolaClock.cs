using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace techsolaWorkClockTimer
{
    public class TechsolaClock : ObservableObject
    {
        private CancellationTokenSource? cancellationTokenSource;

        private string? totalTime;
        public string? TotalTime
        {
            get => totalTime;
            set => Set(ref totalTime, value);
        }

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
        };
        
        private readonly ObservableCollection<TimeSegment> segments = new();

        public ReadOnlyObservableCollection<TimeSegment> Segments => new(segments);

        public TimeSegment? RunningSegment => segments.LastOrDefault() is { End: null } runningSegment
            ? runningSegment
            : null;

        public void Start(string project)
        {
            var breakTime = (new TimeSpan(16, 0, 0) - DateTime.Now.TimeOfDay) -
                                (new TimeSpan(0, 8, 0, 0) - GetCurrentTime(project: null));
            BreakTimeLeft = breakTime.Ticks < 0 ? $@"-{breakTime:hh\:mm\:ss}" : $@"{breakTime:hh\:mm\:ss}";
            // TODO: Non-hard code workday end time
            // TODO: Wire up combobox dropdown for end of day selections
            
            if (RunningSegment is not null)
                throw new InvalidOperationException("Multiple segments must not run at the same time.");

            segments.Add(new TimeSegment(DateTime.Now, project));
            OnPropertyChanged(nameof(RunningSegment));

            cancellationTokenSource = new();

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        TotalTime = $@"{GetCurrentTime(project: null):hh\:mm\:ss}";
                        foreach (var projectTime in Times)
                            projectTime.TotalTime = $@"{GetCurrentTime(projectTime.ProjectName):hh\:mm\:ss}";
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
    }
}