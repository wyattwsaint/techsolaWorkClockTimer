using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accessibility;

namespace techsolaWorkClockTimer
{
    public class TechsolaClock : ObservableObject
    {
        private CancellationTokenSource? cancellationTokenSource;


        private string? techsolaTime;
        public string? TechsolaTime
        {
            get => techsolaTime;
            set => Set(ref techsolaTime, value);
        }


        private string? heritageTime;
        public string? HeritageTime
        {
            get => heritageTime;
            set => Set(ref heritageTime, value);
        }


        private string? exactisTime;
        public string? ExactisTime
        {
            get => exactisTime;
            set => Set(ref exactisTime, value);
        }


        private string? capriCorkTime;
        public string? CapriCorkTime
        {
            get => capriCorkTime;
            set => Set(ref capriCorkTime, value);
        }


        public readonly List<TimeSegment> Segments = new();

        public bool IsRunning => cancellationTokenSource is not null;

        public void Start(string project)
        {
            Segments.Add(new TimeSegment(DateTime.Now, project));
            if (Segments.Any(segment => segment.Project == "Heritage"))
                Segments.Add(new TimeSegment(DateTime.Now, "Heritage"));
            if (Segments.Any(segment => segment.Project == "Exactis"))
                Segments.Add(new TimeSegment(DateTime.Now, "Exactis"));
            if (Segments.Any(segment => segment.Project == "Capri Cork"))
                Segments.Add(new TimeSegment(DateTime.Now, "Capri Cork"));

            cancellationTokenSource = new();
            OnPropertyChanged(nameof(IsRunning));

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        
                        TechsolaTime = $@"{GetCurrentTime("Techsola Internal"):hh\:mm\:ss}";
                        HeritageTime = $@"{GetCurrentTime("Heritage"):hh\:mm\:ss}";
                        ExactisTime = $@"{GetCurrentTime("Exactis"):hh\:mm\:ss}";
                        CapriCorkTime = $@"{GetCurrentTime("Capri Cork"):hh\:mm\:ss}";
                        await Task.Delay(1000, cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                }
            });
        }

        public void Stop()
        {
            Segments.FindLast(segment => segment.Project == "Techsola Internal")!.End = DateTime.Now;
            if (Segments.Any(segment => segment.Project == "Heritage"))
                Segments.FindLast(segment => segment.Project == "Heritage")!.End = DateTime.Now;
            if (Segments.Any(segment => segment.Project == "Exactis"))
                Segments.FindLast(segment => segment.Project == "Exactis")!.End = DateTime.Now;
            if (Segments.Any(segment => segment.Project == "Capri Cork"))
                Segments.FindLast(segment => segment.Project == "Capri Cork")!.End = DateTime.Now;

            cancellationTokenSource.Cancel();

            cancellationTokenSource = null;
            OnPropertyChanged(nameof(IsRunning));
        }

        public TimeSpan GetCurrentTime(string project)
        {
            return Segments.Where(segment => segment.Project == project).Sum(s => (s.End ?? DateTime.Now) - s.Start);
        }
    }
}