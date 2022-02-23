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
        public bool IsHeritageRunning;
        public bool IsExactisRunning;
        public bool IsCapriCorkRunning;

        private string? techsolaTime;
        public string? TechsolaTime
        {
            get => techsolaTime;
            set => Set(ref techsolaTime, value);
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
            Segments.Add(new TimeSegment(DateTime.Now, project));
            if (Segments.Any(segment => segment.Project == "Heritage" && IsHeritageRunning))
                Segments.Add(new TimeSegment(DateTime.Now, "Heritage"));
            if (Segments.Any(segment => segment.Project == "Exactis" && IsExactisRunning))
                Segments.Add(new TimeSegment(DateTime.Now, "Exactis"));
            if (Segments.Any(segment => segment.Project == "Capri Cork" && IsCapriCorkRunning))
                Segments.Add(new TimeSegment(DateTime.Now, "Capri Cork"));


            cancellationTokenSource = new();
            OnPropertyChanged(nameof(IsRunning));

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        TechsolaTime = $@"{GetCurrentTime(project: null):hh\:mm\:ss}";

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