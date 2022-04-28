using System;
using System.Windows.Automation;

namespace techsolaWorkClockTimer
{
    public sealed class ProjectTime : ObservableObject
    {
        public ProjectTime(string projectName, string color, string? workItemOne)
        {
            ProjectName = projectName;
            Color = color;
            WorkItemOne = workItemOne;
        }

        public string ProjectName { get; }
        public string Color { get; }

        private string? workItemOne;
        public string? WorkItemOne
        {
            get => workItemOne;
            set => Set(ref workItemOne, value);
        }

        private string? workItemTwo;
        public string? WorkItemTwo
        {
            get => workItemTwo;
            set => Set(ref workItemTwo, value);
        }

        private string? workItemThree;
        public string? WorkItemThree
        {
            get => workItemThree;
            set => Set(ref workItemThree, value);
        }

        private TimeSpan? time;
        public TimeSpan? Time
        {
            get => time;
            set
            {
                if (value == time) return;
                if (value == new TimeSpan(0,0,0,0)) return;
                Set(ref time, value);
                OnPropertyChanged(nameof(DisplayTime));
                OnPropertyChanged(nameof(DecimalDisplayTime));
            }
        }

        public string? DisplayTime => Time is not null ? $@"{Time:hh\:mm\:ss}" : null;
        public string? DecimalDisplayTime => Time is not null ? $@"{Math.Round(Time.Value.TotalHours * 4, MidpointRounding.ToEven) / 4:0.00}" : null;

    }
}