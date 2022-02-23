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

        public ObservableCollection<ProjectTime> Times { get; } = new()
        {
            new("Techsola Internal"),
            new("Heritage"),
            new("Exactis"),
            new("Capri Cork"),
        };
        
        public readonly List<TimeSegment> Segments = new();
        
        public bool IsRunning => cancellationTokenSource is not null;

        public void Start(string project)
        {
            if (Segments.LastOrDefault() is { End: null })
                throw new InvalidOperationException("Multiple segments must not run at the same time.");

            Segments.Add(new TimeSegment(DateTime.Now, project));

            cancellationTokenSource = new();
            OnPropertyChanged(nameof(IsRunning));

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
            Segments[^1].End = DateTime.Now;

            cancellationTokenSource.Cancel();

            cancellationTokenSource = null;
            OnPropertyChanged(nameof(IsRunning));
        }

        public TimeSpan GetCurrentTime(string? project)
        {
            return Segments
                .Where(segment => project is null || segment.Project == project)
                .Sum(s => (s.End ?? DateTime.Now) - s.Start);
        }
    }
}