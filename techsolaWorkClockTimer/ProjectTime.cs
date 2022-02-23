namespace techsolaWorkClockTimer
{
    public sealed class ProjectTime : ObservableObject
    {
        public ProjectTime(string projectName, string color)
        {
            ProjectName = projectName;
            Color = color;
        }

        public string ProjectName { get; }
        public string Color { get; }

        private string? totalTime;
        public string? TotalTime { get => totalTime; set => Set(ref totalTime, value); }
    }
}