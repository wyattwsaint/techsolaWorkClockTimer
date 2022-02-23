using System;
using System.Collections.Generic;
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

        public const string DefaultProjectName = "Techsola Internal";

        public ObservableCollection<ProjectTime> Times { get; } = new()
        {
            new(DefaultProjectName, "White"),
            new("Heritage", "PaleVioletRed"),
            new("Exactis", "PaleTurquoise"),
            new("Capri Cork", "PaleGoldenrod"),
        };
        
        private readonly List<TimeSegment> segments = new();

        public TimeSegment? RunningSegment => segments.LastOrDefault() is { End: null } runningSegment
            ? runningSegment
            : null;

        public void Start(string project)
        {
            if (segments.LastOrDefault() is { End: null })
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

            cancellationTokenSource.Cancel();

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