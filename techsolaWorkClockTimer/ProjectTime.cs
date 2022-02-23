namespace techsolaWorkClockTimer
{
    public sealed class ProjectTime : ObservableObject
    {
        public ProjectTime(string projectName)
        {
            ProjectName = projectName;
        }

        public string ProjectName { get; }

        private string? totalTime;
        public string? TotalTime { get => totalTime; set => Set(ref totalTime, value); }
    }
}