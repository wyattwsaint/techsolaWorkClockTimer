using System;

namespace techsolaWorkClockTimer
{
    public sealed record TimeSegment(DateTime Start, string Project)
    {
        public DateTime? End { get; set; }
    }
}