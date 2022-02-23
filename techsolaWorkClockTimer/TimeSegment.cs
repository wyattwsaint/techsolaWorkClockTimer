using System;

namespace techsolaWorkClockTimer
{
    public sealed record TimeSegment(DateTime Start, string Project) : ObservableRecord
    {
        private DateTime? end;
        public DateTime? End { get => end; set => Set(ref end, value); }
    }
}