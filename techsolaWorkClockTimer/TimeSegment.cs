using System;

namespace techsolaWorkClockTimer;

public sealed record TimeSegment(DateTime Start, string Project) : ObservableRecord
{
    // Used by dapper
    public TimeSegment(DateTime timeSegmentStart, DateTime timeSegmentEnd, string project) : this(timeSegmentStart, project)
    {
        End = timeSegmentEnd;
    }

    private DateTime? end;

    public DateTime? End
    {
        get => end;
        set => Set(ref end, value);
    }
}